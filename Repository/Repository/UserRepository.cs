using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Models.DTO.Request;
using Repository.Models.DTO.Response;
using Repository.Models.Entities;
using Repository.Models.Enums;
using Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) => _context = context;

        public async Task<UserResponse> Create(UserCreationRequest request)
        {
            // Check if user code already exists
            var existingUserByCode = await _context.Users
                .FirstOrDefaultAsync(u => u.UserCode == request.UserCode);

            if (existingUserByCode != null)
            {
                throw new InvalidOperationException($"User with code '{request.UserCode}' already exists.");
            }

            // Check if username already exists
            var existingUserByName = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (existingUserByName != null)
            {
                throw new InvalidOperationException($"Username '{request.UserName}' already exists.");
            }

            // Check if email already exists
            var existingUserByEmail = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUserByEmail != null)
            {
                throw new InvalidOperationException($"Email '{request.Email}' already exists.");
            }

            // Check if role exists
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleId == request.RoleId);

            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID '{request.RoleId}' not found.");
            }

            // Check if warehouse exists
            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(w => w.WarehouseCode == request.WarehouseCode);

            if (warehouse == null)
            {
                throw new KeyNotFoundException($"Warehouse with code '{request.WarehouseCode}' not found.");
            }

            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                UserCode = request.UserCode,
                UserName = request.UserName,
                FullName = request.FullName,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password), // Hash password
                Role = role,
                Warehouse = warehouse,
                Status = UserStatus.active,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserResponse
            {
                Id = user.UserId,
                UserCode = user.UserCode,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                WarehouseCode = user.Warehouse.WarehouseCode,
                Role = user.Role
            };
        }
    }
}
