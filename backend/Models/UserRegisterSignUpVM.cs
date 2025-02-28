using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class UserRegisterSignUpVM
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
