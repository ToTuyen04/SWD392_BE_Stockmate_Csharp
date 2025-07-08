using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Repository;
using Repository.Repository.Interface;
using Service.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService() => _unitOfWork = new UnitOfWork();
        /// <summary>
        /// Create a new user
        /// Equivalent to Java: createUser(UserCreationRequest request)
        /// </summary>
        public async Task<UserResponse> CreateUser(UserCreationRequest userRequest)
        {
            try
            {
                return await _unitOfWork.UserRepository.Create(userRequest);
            }
            catch (InvalidOperationException ex)
            {
                // User code/username/email already exists - equivalent to USER_CODE_EXIST/USER_EXIST/EMAIL_EXIST in Java
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (KeyNotFoundException ex)
            {
                // Role or Warehouse not found - equivalent to ROLE_NOT_FOUND/WAREHOUSE_NOT_FOUND in Java
                throw new KeyNotFoundException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user.", ex);
            }
        }

        public Task<UserResponse> GetMyProfile(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
