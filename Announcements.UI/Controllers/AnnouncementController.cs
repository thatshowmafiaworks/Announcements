using Announcements.UI.Models;
using Announcements.UI.Models.DTOs;
using Announcements.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Announcements.UI.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementsService _api;
        private readonly ILogger<AnnouncementController> _logger;
        public AnnouncementController(IAnnouncementsService api, ILogger<AnnouncementController> logger)
        {
            _api = api;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AnnouncementDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"User tried to create new announcement with invalid data:(numberof mistakes:{ModelState.ErrorCount})");
                return View(dto);
            }
            try
            {
                var id = await _api.Create(dto);
                TempData["SuccessMessage"] = $"Successfuly created new announcement with title:'{dto.Title}'.";
                return RedirectToAction(nameof(All));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Creating went wrong with Exception:{ex.Message}\n with Inner:{ex.InnerException?.Message}");
                TempData["ErrorMessage"] = $"Something went wrong, please try recheck data.";
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = new AnnouncementsDisplayModel
            {
                List = await _api.ReadAll()
            };
            _logger.LogInformation($"User retrieved all announcements (Count:{model.List.Count})");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["ErrorMessage"] = "id is null or empty";
                return RedirectToAction(nameof(All));
            }
            try
            {
                var item = await _api.Read(id);
                return View(item);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong, try again please.";
                _logger.LogError($"Updating(GET) went wrong with Exception:{ex.Message}\n with Inner:{ex.InnerException?.Message}");
                return RedirectToAction(nameof(All));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(Announcement item)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Recheak data please";
                return View(item);
            }
            try
            {
                await _api.Update(item);
                TempData["SuccessMessage"] = $"Succesfully updated announcement with title :'{item.Title}'";
                return RedirectToAction(nameof(All));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong, try again please.";
                _logger.LogError($"Updating(POST) went wrong with Exception:{ex.Message}\n with Inner:{ex.InnerException?.Message}");
                return View(item);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["ErrorMessage"] = "id is null or empty";
            }
            try
            {
                await _api.Delete(id);
                TempData["SuccessMessage"] = $"Succesfully deleted announcement with id :'{id}'";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Deleting of id:{id} went wrong with Exception:{ex.Message}\n with Inner:{ex.InnerException?.Message}");
                TempData["ErrorMessage"] = "Something went wrong, try again please.";
            }
            return RedirectToAction(nameof(All));
        }
    }
}
