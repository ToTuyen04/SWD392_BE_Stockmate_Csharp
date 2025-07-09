using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;

namespace Service.Service.Interface
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request);
        Task<IntrospectResponse> Introspect(IntrospectRequest request);
        Task Logout(LogoutRequest request);
    }
}
