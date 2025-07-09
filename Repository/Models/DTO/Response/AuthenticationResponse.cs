using Repository.Models.Entities;

namespace Repository.Models.DTO.Response
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public bool Authenticated { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
    }
}
