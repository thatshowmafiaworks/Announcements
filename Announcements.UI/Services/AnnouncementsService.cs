using Announcements.UI.Models;
using Announcements.UI.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace Announcements.UI.Services
{
    public class AnnouncementsService : IAnnouncementsService
    {
        private readonly string api;
        private readonly ILogger<AnnouncementsService> logger;
        private readonly HttpClient client;
        public AnnouncementsService(IConfiguration config, ILogger<AnnouncementsService> logger, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.client = httpClientFactory.CreateClient();
            if (string.IsNullOrEmpty(config["ApiLink"]))
                logger.LogCritical($"ApiLink is empty: '{config["ApiLink"]}'");
            this.api = config["ApiLink"];
        }

        public async Task Delete(string id)
        {
            try
            {
                var response = await client.DeleteAsync(api + "/" + id);
                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation($"Deleted announcement with id:{id}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Reading with id:'{id}' went wrong\nwith exception:{ex.Message}\nwith Inner:{ex.InnerException?.Message}");
            }
        }

        public async Task<Announcement> Read(string id)
        {
            try
            {
                var response = await client.GetAsync(api + "/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var opts = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var content = await response.Content.ReadAsStringAsync();
                    var item = JsonSerializer.Deserialize<Announcement>(content, opts);
                    return item;
                }
                else
                {
                    logger.LogError($"Api returned StatusCode:{response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Reading with id:'{id}' went wrong\nwith exception:{ex.Message}\nwith Inner:{ex.InnerException?.Message}");
            }
            return null;
        }

        public async Task<List<Announcement>> ReadAll()
        {
            try
            {
                var response = await client.GetAsync(api);
                if (response.IsSuccessStatusCode)
                {
                    var opts = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var content = await response.Content.ReadAsStringAsync();
                    var list = JsonSerializer.Deserialize<List<Announcement>>(content, opts);
                    return list;
                }
                else
                {
                    logger.LogError($"Api returned StatusCode:{response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Reading all went wrong with exception:{ex.Message}\nwith Inner:{ex.InnerException?.Message}");
            }
            return new List<Announcement>();
        }

        public async Task Update(Announcement item)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(api + "/" + item.Id, content);
                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation($"Updated announcement with id:'{item.Id}'");
                }
                else
                {
                    logger.LogError($"Updating id:'{item.Id}' went wrong");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Updating id:'{item.Id}' went wrong with exception:{ex.Message}\nwith Inner:{ex.InnerException?.Message}");
            }
        }
        public async Task<string> Create(AnnouncementDto model)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(api, content);
                if (response.IsSuccessStatusCode)
                {
                    var id = await response.Content.ReadAsStringAsync();
                    logger.LogInformation($"Created with id:{id}");
                    return id;
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Creating went wrong with exception:{ex.Message}\nwith Inner:{ex.InnerException?.Message}");
            }
            return null;
        }
    }
}
