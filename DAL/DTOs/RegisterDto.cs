using System.ComponentModel.DataAnnotations;


namespace DAL.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }

         [Required]
        public string Password { get; set; }

        public string VerificationCode{ get; set; }
    }
}