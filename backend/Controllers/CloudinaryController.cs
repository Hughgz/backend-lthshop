using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using backend.Models;

namespace backend.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class CloudinaryController : ControllerBase
        {
            private readonly Cloudinary _cloudinary;

            public CloudinaryController(IConfiguration configuration)
            {
                var cloudName = configuration["Cloudinary:CloudName"];
                var apiKey = configuration["Cloudinary:ApiKey"];
                var apiSecret = configuration["Cloudinary:ApiSecret"];

                _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
            }

            [HttpPost("upload")]
            public async Task<IActionResult> Upload(IFormFile file)
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "File không hợp lệ." });

                // Đọc tệp vào Stream
                using var stream = file.OpenReadStream();

                // Tạo yêu cầu upload
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "uploads", // Thư mục trên Cloudinary (tuỳ chọn)
                    PublicId = Path.GetFileNameWithoutExtension(file.FileName), // Tên tệp (tuỳ chọn)
                    Overwrite = true // Ghi đè nếu đã tồn tại
                };

                // Thực hiện upload
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return StatusCode((int)uploadResult.StatusCode, new { message = "Upload thất bại.", error = uploadResult.Error.Message });
                }

                // Trả kết quả
                var result = new CloudinaryUploadModel
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.Url.ToString()
                };

                return Ok(result);
            }
        }
    }
