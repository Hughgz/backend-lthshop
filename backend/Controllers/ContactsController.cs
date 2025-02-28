using backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net.Http;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ContactsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] ContactData contactData)
        {
            var accessToken = "CIO22--_MhIUAAEBUAAA-SIAAED8BwEA4AcAAAQYhPiMFyD-7-cjKJvfmgEyFO60wGh_IqoqRXU90wLtoLl895i7OkEAAABBAAAAAMAHAAAAAAAAAIYAAAAAAAAADAAggA8APgDgMQAAAAAAwP__HwAQ8AMAAID__wMAAAAAAOABAADsH0IUrjQDj6dckWVw53tdYKTXoHLbNjtKA25hMVIAWgBgAA";  // Thay thế bằng Access Token HubSpot của bạn

            var url = "https://api.hubapi.com/contacts/v1/contact";
            var requestData = new
            {
                properties = new[]
                {
                    new { property = "email", value = contactData.Email },
                    new { property = "firstname", value = contactData.FirstName },
                    new { property = "lastname", value = contactData.LastName },
                    new { property = "phone", value = contactData.Phone }
                }
            };

            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(requestData),
                Encoding.UTF8,
                "application/json"
            );

            // Tạo HttpRequestMessage để thêm header Authorization
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            request.Headers.Add("Authorization", $"Bearer {accessToken}");

            try
            {
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
