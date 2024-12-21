using backend.Helper;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmsController : ControllerBase
    {
        private readonly TwilioService _twilioService;

        public SmsController(TwilioService twilioService)
        {
            _twilioService = twilioService;
        }

        /// <summary>
        /// API endpoint để gửi OTP qua SMS.
        /// </summary>
        /// <param name="toPhoneNumber">Số điện thoại cần gửi OTP.</param>
        /// <returns>Kết quả gửi OTP.</returns>
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SmsRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ToPhoneNumber))
            {
                return BadRequest("Phone number is required.");
            }

            // Generate OTP
            string otp = OtpHelper.GenerateOtp();

            // Send OTP via Twilio
            bool result = await _twilioService.SendOtpAsync(request.ToPhoneNumber, otp);

            if (result)
            {
                return Ok(new { message = "OTP sent successfully.", otp });
            }
            else
            {
                return StatusCode(500, "Failed to send OTP.");
            }
        }
    }

    /// <summary>
    /// Request model cho API gửi SMS.
    /// </summary>
    public class SmsRequest
    {
        public string ToPhoneNumber { get; set; }
    }
}
