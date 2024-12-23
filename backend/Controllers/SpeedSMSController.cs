using AutoMapper;
using backend.Entities;
using backend.Helper;
using backend.Models;
using backend.Repositories.EntitiesRepo;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpeedSMSController : ControllerBase
    {
            private readonly SpeedSMSService _speedSMS;

            public SpeedSMSController()
            {
                // Initialize SpeedSMSAPI with your token
                string accessToken = "rnM1l_pkHyoKbgIIkLUqJItOAQwtG7T_"; // Replace with your actual API token
                _speedSMS = new SpeedSMSService(accessToken);
            }

            [HttpPost("send")]
            public IActionResult SendSms([FromBody] SMSViewModel request)
            {
                try
                {
                    if (request == null || request.Phones == null || request.Phones.Length == 0)
                    {
                        return BadRequest("Invalid request: Phones and content are required.");
                    }

                    string otp = OtpHelper.GenerateOtp();
                    string sender = "54eb95390d21757c";

                    var response = _speedSMS.sendSMS(
                        request.Phones,
                        $"OTP Code LTH Store: {otp}",
                        2,
                        sender
                    );

                    return Ok(new { message = otp, response });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
    }
