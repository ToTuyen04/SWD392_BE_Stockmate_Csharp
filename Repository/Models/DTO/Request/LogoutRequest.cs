using System.ComponentModel.DataAnnotations;

namespace Repository.Models.DTO.Request
{
    public class LogoutRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
