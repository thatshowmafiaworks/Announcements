using Announcements.Api.Models;
using Microsoft.Data.SqlClient;

namespace Announcements.Api.Services
{
    public class AnnouncementsRepository : IAnnouncementsRepository
    {
        private readonly SqlConnection _connection;
        private readonly ILogger _logger;
        public AnnouncementsRepository(IConfiguration config, ILogger logger)
        {
            _connection = new SqlConnection(config["AzureDb"]);
            _logger = logger;
        }

        public async Task Create(Announcement item)
        {
            try
            {
                using (var command = new SqlCommand("CreateAnnouncement", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", item.Id);
                    command.Parameters.AddWithValue("@Title", item.Title);
                    command.Parameters.AddWithValue("@Description", item.Description);
                    command.Parameters.AddWithValue("@CreatedDate", item.CreatedDate);
                    command.Parameters.AddWithValue("@UpdatedDate", item.UpdatedDate);
                    command.Parameters.AddWithValue("@Status", item.Status);
                    command.Parameters.AddWithValue("@Category", item.Category);
                    command.Parameters.AddWithValue("@SubCategory", item.SubCategory);

                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Creating went wrong with\nException:{ex.Message} \n and Inner: {ex.InnerException.Message}");
            }
        }

        public async Task Delete(string id)
        {
            try
            {
                using (var command = new SqlCommand("DeleteAnnouncement", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Deleting of id:{id} went wrong with\nException:{ex.Message} \n and Inner: {ex.InnerException.Message}");
            }
        }

        public async Task<Announcement> Read(string id)
        {
            try
            {
                using (var command = new SqlCommand("ReadAnnouncement", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    await _connection.OpenAsync();
                    using var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        return MapToAnnouncement(reader);
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Reading by id:{id} went wrong with\nException:{ex.Message} \n and Inner: {ex.InnerException.Message}");
                return null;
            }
        }

        public async Task<List<Announcement>> ReadAll()
        {
            var list = new List<Announcement>();
            try
            {
                using (var command = new SqlCommand("ReadAllAnnouncements", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    await _connection.OpenAsync();
                    using var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        list.Add(MapToAnnouncement(reader));
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Reading all announcements went wrong with\nException:{ex.Message} \n and Inner: {ex.InnerException.Message}");
                return new List<Announcement>();
            }
        }

        public async Task Update(Announcement item)
        {
            try
            {
                using (var command = new SqlCommand("UpdateAnnouncement", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", item.Id);
                    command.Parameters.AddWithValue("@Title", item.Title);
                    command.Parameters.AddWithValue("@Description", item.Description);
                    command.Parameters.AddWithValue("@CreatedDate", item.CreatedDate);
                    command.Parameters.AddWithValue("@UpdatedDate", item.UpdatedDate);
                    command.Parameters.AddWithValue("@Status", item.Status);
                    command.Parameters.AddWithValue("@Category", item.Category);
                    command.Parameters.AddWithValue("@SubCategory", item.SubCategory);

                    await _connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Updating of id:{item.Id} went wrong with\nException:{ex.Message} \n and Inner: {ex.InnerException.Message}");
            }
        }

        private Announcement MapToAnnouncement(SqlDataReader reader)
            => new Announcement
            {
                Id = reader["Id"].ToString(),
                Title = reader["Title"].ToString(),
                Description = reader["Description"].ToString(),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]),
                Status = reader["Status"].ToString(),
                Category = reader["Category"].ToString(),
                SubCategory = reader["SubCategory"].ToString(),
            };
    }
}
