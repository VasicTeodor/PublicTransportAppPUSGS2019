using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using PublicTransport.Api.Models;

namespace PublicTransport.Api.Data
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public Seed(UserManager<User> userManager,RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var roles = new List<Role>
                {
                    new Role() {Name = "Passenger"},
                    new Role() {Name = "Controller"},
                    new Role() {Name = "Admin"}
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role);
                }

                for (int i = 0; i < 5; i++)
                {
                    var user = new User()
                    {
                        UserName = $"User{i}",
                        Name = $"Pera {i}",
                        Surname = $"Peric {i}"
                    };

                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "Passenger").Wait();
                }

                for (int i = 0; i < 2; i++)
                {
                    var adminUser = new User
                    {
                        UserName = $"Admin{i}"
                    };

                    IdentityResult result = _userManager.CreateAsync(adminUser, "admin").Result;

                    if (result.Succeeded)
                    {
                        var admin = _userManager.FindByNameAsync($"Admin{i}").Result;
                        _userManager.AddToRolesAsync(admin, new[] { "Admin", "Controller" }).Wait();
                    }
                }

                for (int i = 0; i < 2; i++)
                {
                    var controller = new User
                    {
                        UserName = $"Controller{i}"
                    };

                    IdentityResult result = _userManager.CreateAsync(controller, "controller").Result;

                    if (result.Succeeded)
                    {
                        var admin = _userManager.FindByNameAsync($"Controller{i}").Result;
                        _userManager.AddToRoleAsync(admin, "Controller").Wait();
                    }
                }

            }
        }
    }
}