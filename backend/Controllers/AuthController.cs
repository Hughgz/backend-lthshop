using backend.Dtos;
using backend.Entities;
using backend.Helper;
using backend.Models;
using backend.Repositories.AuthRepo;
using backend.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IAuthRepo _authenRepo;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly EcommerceDBContext _context;

        private static readonly Dictionary<string, string> OtpStore = new();
        public AuthenController(IAuthRepo authenRepo, IConfiguration configuration, IEmailService emailService, EcommerceDBContext context)
        {
            _authenRepo = authenRepo;
            _configuration = configuration;
            _emailService = emailService;
            _context = context;
          _context = context;
        }

        // POST: api/Authen/Login (Login User)
        [HttpPost("login-user")]
        public async Task<ActionResult> Login([FromBody] LoginRequestVM loginRequest)
        {
            var user = await _authenRepo.ValidateUserCredentialsAsync(loginRequest.Email, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);
            SetAuthCookie(token);

            return Ok(new { Token = token, User = user });
        }

        [HttpPost("login-customer")]
        public async Task<ActionResult> LoginCustomer([FromBody] LoginRequestVM loginRequest)
        {
            var customer = await _authenRepo.ValidateCustomerCredentialsAsync(loginRequest.Email, loginRequest.Password);
            if (customer == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(customer);
            SetAuthCookie(token);

            return Ok(new { Token = token, Customer = customer });
        }

        // POST: api/Authen/Register(Register Customer)
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] CustomerSignUpVM registerRequest)
        {
            var customer = await _authenRepo.RegisterCustomerAsync(registerRequest);
            if (customer == null)
            {
                return BadRequest("Registration failed.");
            }


            return Ok(new { customer });
        }
        [HttpPost("registerUser")]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterSignUpVM registerRequest)
        {
            var user = await _authenRepo.RegisterUserAsync(registerRequest);
            if (user == null)
            {
                return BadRequest("Registration failed.");
            }


            return Ok(new { user });
        }


        // POST: api/Authen/Logout (Logout User)
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthCookie");
            return Ok("Logged out successfully.");
        }

        // POST: api/Authen/refresh-token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            // Retrieve the existing token from the cookie
            var currentToken = Request.Cookies["AuthCookie"];
            if (string.IsNullOrEmpty(currentToken))
            {
                return Unauthorized("No token found.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                // Validate and decode the current token to extract the claims
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false, // We don't want to validate the expiration yet
                    ClockSkew = TimeSpan.Zero // Remove clock skew allowance
                };

                var claimsPrincipal = tokenHandler.ValidateToken(currentToken, tokenValidationParameters, out var validatedToken);
                var roleClaim = claimsPrincipal.FindFirst("Role")?.Value;

                if (roleClaim == null)
                {
                    return Unauthorized("Invalid token.");
                }

                // Based on the role, fetch the correct user or customer
                object userOrCustomer;

                if (roleClaim == "User")
                {
                    var usernameClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                    userOrCustomer = await _authenRepo.GetUserByEmailAsync(usernameClaim);
                }
                else if (roleClaim == "Customer")
                {
                    var emailClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                    userOrCustomer = await _authenRepo.GetCustomerByEmailAsync(emailClaim);
                }
                else
                {
                    return Unauthorized("Invalid token role.");
                }

                // Regenerate a new token
                var newToken = GenerateJwtToken(userOrCustomer);
                SetAuthCookie(newToken);

                return Ok(new { Token = newToken });
            }
            catch (Exception ex)
            {
                return Unauthorized($"Invalid token: {ex.Message}");
            }
        }

        private string GenerateJwtToken(object userOrCustomer)
        {
            // Check the type of the user or customer and generate appropriate claims
            Claim[] claims;

            if (userOrCustomer is User user)
            {
                claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Role", "User")
                };
            }
            else if (userOrCustomer is CustomerReadDto customer)
            {
                claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, customer.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Role", "Customer")
                };
            }
            else
            {
                throw new ArgumentException("Invalid object type");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void SetAuthCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddDays(1)
            };
            Response.Cookies.Append("AuthCookie", token, cookieOptions);
        }
        [HttpPost("verify-email")]
        public async Task<ActionResult> VerifyEmail([FromBody] VerifyEmailRequestVM request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Invalid token.");
            }

            var customer = await _authenRepo.GetCustomerByVerificationTokenAsync(request.Token);

            if (customer == null)
            {
                return BadRequest("Invalid or expired token.");
            }

            // Xác nhận email
            customer.EmailConfirmed = true;
            customer.EmailVerificationToken = string.Empty; // Xóa token sau khi xác minh
            _context.Customers.Update(customer); // Đảm bảo cập nhật vào DbContext
            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

            return Ok("Email verified successfully.");
        }


        //forgot password
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequestVM request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email cannot be null or empty.");
            }

            var customer = await _authenRepo.GetCustomerByEmailAsync(request.Email);
            if (customer == null)
            {
                return BadRequest("Email not registered.");
            }

            var otp = OtpHelper.GenerateOtp();
            OtpStore[request.Email] = otp;

            try
            {
                var subject = "Reset Your Password";
                var body = $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 10px; overflow: hidden;'>
                <div style='background-color: #4CAF50; padding: 20px; text-align: center; color: #fff;'>
                    <h1 style='margin: 0; font-size: 24px;'>Chào mừng bạn đến với gia đình LTH Store</h1>
                </div>
                <div style='padding: 30px; background-color: #f7f7f7;'>
                    <p style='font-size: 18px; margin: 0 0 20px 0;'>Hi there,</p>
                    <p style='margin: 0 0 20px 0;'>Đây là mã OTP của bạn, không chia sẽ cho bất kì ai: </p>
                    <p>OTP: {otp}</p>
                    <p>Thanks,</p>
                    <p>LTH Store Team</p>
                </div>
                <div style='background-color: #4CAF50; padding: 10px; text-align: center; color: #fff; font-size: 14px;'>
                    <p style='margin: 0;'>Need help? Contact our <a href='mailto:support@hilocinema.com' style='color: #fff; text-decoration: underline;'>support team</a>.</p>
                </div>
            </div>";
                await _emailService.SendEmailAsync(request.Email, subject, body);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }

            return Ok("OTP sent to your email.");
        }


        // Step 2: Xác minh OTP
        [HttpPost("verify-otp")]
        public ActionResult VerifyOtp([FromBody] VerifyOtpRequestVM request)
        {
            if (!OtpStore.ContainsKey(request.Email))
            {
                return BadRequest("OTP not found.");
            }

            var storedOtp = OtpStore[request.Email];

            if (storedOtp != request.Otp)
            {
                return BadRequest("Invalid OTP.");
            }

            // OTP is valid, you can proceed with the next steps.
            return Ok("OTP verified successfully.");
        }


        // Step 3: Đổi mật khẩu
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequestVM request)
        {
            if (!OtpStore.TryGetValue(request.Email, out var validOtp) || validOtp != request.Otp)
            {
                return BadRequest("Invalid or expired OTP.");
            }

            // Xóa OTP sau khi sử dụng
            OtpStore.Remove(request.Email);

            // Đổi mật khẩu
            var result = await _authenRepo.ChangeCustomerPasswordAsync(request.Email, request.NewPassword);
            if (!result)
            {
                return StatusCode(500, "Password change failed.");
            }

            return Ok("Password changed successfully.");
        }

    }
}
