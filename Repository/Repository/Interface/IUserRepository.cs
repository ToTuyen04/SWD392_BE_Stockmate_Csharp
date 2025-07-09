using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Entities;

namespace Repository.Repository.Interface
{
    public interface IUserRepository
    {
        Task<UserResponse> Create(UserCreationRequest request);
        Task<User> GetByEmailAsync(string email);
    }
}
