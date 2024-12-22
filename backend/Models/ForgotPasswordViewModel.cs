namespace backend.Models
{
        public class ForgotPasswordRequestVM
        {
            public string Email { get; set; }
        }

        public class VerifyOtpRequestVM
        {
            public string Email { get; set; }
            public string Otp { get; set; }
        }

        public class ChangePasswordRequestVM
        {
            public string Email { get; set; }
            public string Otp { get; set; }
            public string NewPassword { get; set; }
       }
  }
