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

                var userList = new List<User>();

                for (int i = 0; i < 5; i++)
                {
                    var user = new User()
                    {
                        UserName = $"User{i}",
                        Name = $"Pera {i}",
                        Surname = $"Peric {i}"
                    };

                    userList.Add(user);
                   
                }

                foreach (var user in userList)
                {
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "Passenger").Wait();
                }

                var admin = new User { UserName = "Admin" };
                IdentityResult result = _userManager.CreateAsync(admin, "password").Result;

                if (result.Succeeded)
                {
                    var adm = _userManager.FindByNameAsync("Admin").Result;
                    _userManager.AddToRolesAsync(adm, new[] { "Admin", "Controller" }).Wait();
                }

                var controller = new User { UserName = "Controller" };
                IdentityResult res = _userManager.CreateAsync(controller, "password").Result;

                if (res.Succeeded)
                {
                    var con = _userManager.FindByNameAsync("Controller").Result;
                    _userManager.AddToRoleAsync(con, "Controller" ).Wait();
                }

            }
        }
    }
}