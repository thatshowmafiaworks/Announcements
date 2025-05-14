using Announcements.Api.Models;
using Announcements.Api.Models.DTOs;
using Announcements.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Announcements.Api.Controllers
{
    [Controller]
    public class AnnouncementsController(IAnnouncementsRepository repo, ILogger<AnnouncementsController> logger) : ControllerBase
    {
        [Route("announcement")]
        [HttpGet]
        public async Task<IActionResult> ReadAll()
        {
            try
            {
                var response = await repo.ReadAll();
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError($"Getting All items went wrong with\nException:{ex.Message} \n and Inner: {ex.InnerException.Message}");
                return StatusCode(500, new { error = "Something went wrong, please try later" });
            }
        }
        [Route("announcement/{id}")]
        [HttpGet]
        public async Task<IActionResult> Read([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                logger.LogWarning($"Id is null or WhiteSpace");
                return BadRequest(new { error = "Id cant be empty" });
            }
            try
            {
                var response = await repo.Read(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError($"Getting by Id went wrong with\nException:{ex.Message} \n and Inner: {ex.InnerException.Message}");
                return StatusCode(500, new { error = "Something went wrong, please try later" });
            }
        }
        [Route("announcement")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AnnouncementDto model)
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning($"Model came with errors:{ModelState.ErrorCount}");
                return BadRequest(new { error = "Bad data, try again later" });
            }
            try
            {
                var announcement = MapToAnnouncement(model);
                await repo.Create(announcement);
                return Ok(announcement.Id);
            }
            catch (Exception ex)
            {
                logger.LogError($"Creating went wrong with\nException:{ex.Message} \n and Inner: {ex.InnerException.Message}");
                return StatusCode(500, new { error = "Something went wrong, please try later" });
            }
        }
        [Route("announcement/{id}")]
        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] AnnouncementDto model)
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning($"Model came with errors:{ModelState.ErrorCount}");
                return BadRequest(new { error = "Bad data, try again later" });
            }
            try
            {
                var toUpdate = await repo.Read(id);
                toUpdate.Title = model.Title;
                toUpdate.Description = model.Description;
                toUpdate.UpdatedDate = DateTime.UtcNow;
                toUpdate.Category = model.Category;
                toUpdate.SubCategory = model.SubCategory;
                await repo.Update(toUpdate);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Updating went wrong with\nException:{ex.Message} \n and Inner: {ex.InnerException?.Message}");
                return StatusCode(500, new { error = "Something went wrong, please try later" });
            }
        }

        [Route("announcement/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                logger.LogWarning($"Id is null or WhiteSpace");
                return BadRequest(new { error = "Id cant be empty" });
            }
            try
            {
                await repo.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Deleting went wrong with\nException:{ex.Message} \n and Inner: {ex.InnerException?.Message}");
                return StatusCode(500, new { error = "Something went wrong, please try later" });
            }
        }

        private Announcement MapToAnnouncement(AnnouncementDto dto)
            => new Announcement
            {
                Id = Guid.NewGuid().ToString(),
                Title = dto.Title,
                Description = string.IsNullOrWhiteSpace(dto.Description) ? "" : dto.Description,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                Status = "Active",
                Category = dto.Category,
                SubCategory = dto.SubCategory
            };
    }
}
