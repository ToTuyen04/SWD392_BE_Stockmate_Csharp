using Microsoft.EntityFrameworkCore;
using Repository.Models.Entities;
using Repository.Models.Enums;

namespace Repository.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(ApplicationDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Seed Roles
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<Role>
                {
                    new Role { RoleId = "AD", RoleType = "ADMIN", RoleName = "admin" },
                    new Role { RoleId = "MA", RoleType = "MANAGER", RoleName = "manager" },
                    new Role { RoleId = "ST", RoleType = "STAFF", RoleName = "staff" }
                };

                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }

            // Seed Warehouses
            if (!await context.Warehouses.AnyAsync())
            {
                var warehouses = new List<Warehouse>
                {
                    new Warehouse { WarehouseId = "1", WarehouseCode = "W1", WarehouseName = "Ware house 1", Address = "Ha Noi" },
                    new Warehouse { WarehouseId = "2", WarehouseCode = "W2", WarehouseName = "Ware house 2", Address = "Ho Chi Minh" },
                    new Warehouse { WarehouseId = "3", WarehouseCode = "W3", WarehouseName = "Ware house 3", Address = "Can Tho" },
                    new Warehouse { WarehouseId = "4", WarehouseCode = "WH0001", WarehouseName = "Warehouse WH0001", Address = "Ha Noi" }
                };

                await context.Warehouses.AddRangeAsync(warehouses);
                await context.SaveChangesAsync();
            }

            // Seed Admin User if not exists
            if (!await context.Users.AnyAsync(u => u.Email == "admin@gmail.com"))
            {
                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.RoleType == "ADMIN");
                var warehouse = await context.Warehouses.FirstOrDefaultAsync(w => w.WarehouseCode == "WH0001");

                if (adminRole != null && warehouse != null)
                {
                    var adminUser = new User
                    {
                        UserId = Guid.NewGuid().ToString(),
                        UserCode = "USR001",
                        UserName = "admin",
                        FullName = "Administrator",
                        Email = "admin@gmail.com",
                        Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                        RoleId = adminRole.RoleId,
                        WarehouseCode = warehouse.WarehouseCode,
                        Status = UserStatus.active,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await context.Users.AddAsync(adminUser);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
