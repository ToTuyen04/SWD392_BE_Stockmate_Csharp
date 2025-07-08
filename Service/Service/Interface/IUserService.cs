using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interface
{
    public interface IUserService
    {
        Task<UserResponse> CreateUser(UserCreationRequest userRequest);
        Task<UserResponse> GetMyProfile(string userId);
    }
}
