using System.ComponentModel.DataAnnotations;

namespace Repository.Models.DTO.Request
{
    public class IntrospectRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
