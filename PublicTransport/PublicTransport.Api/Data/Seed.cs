using System;
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
        private readonly DataContext _context;

        public Seed(UserManager<User> userManager,RoleManager<Role> roleManager, DataContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
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
                    _roleManager.CreateAsync(role).Wait();
                }

                var userList = new List<User>();

                for (int i = 0; i < 5; i++)
                {
                    var user = new User()
                    {
                        UserName = $"User{i}",
                        Name = $"Pera {i}",
                        Surname = $"Peric {i}",
                        Email = $"user{i}@gmail.com"
                    };

                    userList.Add(user);
                   
                }

                _userManager.CreateAsync(userList[0], "password").Wait();
                _userManager.AddToRoleAsync(userList[0], "Passenger").Wait();

                _userManager.CreateAsync(userList[1], "password").Wait();
                _userManager.AddToRoleAsync(userList[1], "Passenger").Wait();

                _userManager.CreateAsync(userList[2], "password").Wait();
                _userManager.AddToRoleAsync(userList[2], "Passenger").Wait();

                _userManager.CreateAsync(userList[3], "password").Wait();
                _userManager.AddToRoleAsync(userList[3], "Passenger").Wait();

                _userManager.CreateAsync(userList[4], "password").Wait();
                _userManager.AddToRoleAsync(userList[4], "Passenger").Wait();

                var admin = new User { UserName = "Admin", Email = "admin@publictransport.com"};
                IdentityResult result = _userManager.CreateAsync(admin, "password").Result;

                if (result.Succeeded)
                {
                    var adm = _userManager.FindByNameAsync("Admin").Result;
                    _userManager.AddToRolesAsync(adm, new[] { "Admin", "Controller" }).Wait();
                }

                var controller = new User { UserName = "Controller", Email = "controller@publictransport.com"};
                IdentityResult res = _userManager.CreateAsync(controller, "password").Result;

                if (res.Succeeded)
                {
                    var con = _userManager.FindByNameAsync("Controller").Result;
                    _userManager.AddToRoleAsync(con, "Controller" ).Wait();
                }
            }
        }

        public void SeedStations()
        {
            Station station = new Station()
            {
                Address = new Address()
                {
                    City = "Novi Sad",
                    Number = "122",
                    Street = "Bul Oslobodjenja"
                },
                Location = new Location()
                {
                    X = 223.12333,
                    Y = 123.44221
                },
                Name = "Lutrija"
            };

            //_context.Add(station);

            TimeTable timeTable1 = new TimeTable()
            {
                Day = "Working days",
                Departures = "12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32",
                Type = "In City",
                Line = new Line()
                {
                    Name = "7A:Stanica-Lutrija-Liman",
                    LineNumber = 7,
                    Buses = new List<Bus>()
                    {
                        new Bus()
                        {
                            BusNumber = 244,
                            InUse = true,
                        },
                        new Bus()
                        {
                            BusNumber = 222,
                            InUse = true,
                        },
                        new Bus()
                        {
                            BusNumber = 234,
                            InUse = true,
                        }
                    }
                }
            };
            _context.Add(timeTable1);

            TimeTable timeTable2 = new TimeTable()
            {
                Day = "Working days",
                Departures = "12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32",
                Type = "In City",
                Line = new Line()
                {
                    Name = "7B:Liman-Lutrija-Stanica",
                    LineNumber = 7,
                    Buses = new List<Bus>()
                    {
                        new Bus()
                        {
                            BusNumber = 241,
                            InUse = true,
                        },
                        new Bus()
                        {
                            BusNumber = 221,
                            InUse = true,
                        },
                        new Bus()
                        {
                            BusNumber = 231,
                            InUse = true,
                        }
                    }
                }
            };
            _context.Add(timeTable2);

            TimeTable timeTable3 = new TimeTable()
            {
                Day = "Working days",
                Departures = "12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32",
                Type = "In City",
                Line = new Line()
                {
                    Name = "12:Centar-Telep",
                    LineNumber = 12,
                    Buses = new List<Bus>()
                    {
                        new Bus()
                        {
                            BusNumber = 111,
                            InUse = true,
                        },
                        new Bus()
                        {
                            BusNumber = 112,
                            InUse = true,
                        },
                        new Bus()
                        {
                            BusNumber = 113,
                            InUse = true,
                        }
                    }
                }
            };
            _context.Add(timeTable3);

            TimeTable timeTable4 = new TimeTable()
            {
                Day = "Working days",
                Departures = "12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32",
                Type = "In City",
                Line = new Line()
                {
                    Name = "12:Telep-Centar",
                    LineNumber = 12,
                    Buses = new List<Bus>()
                    {
                        new Bus()
                        {
                            BusNumber = 241,
                            InUse = true,
                        },
                        new Bus()
                        {
                            BusNumber = 221,
                            InUse = true,
                        },
                        new Bus()
                        {
                            BusNumber = 231,
                            InUse = true,
                        }
                    }
                }
            };
            _context.Add(timeTable4);

            TimeTable timeTable5 = new TimeTable()
            {
                Day = "Working days",
                Departures = "12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32",
                Type = "In City",
                Line = new Line()
                {
                    Name = "8:Novo Naselje-Centar-Liman",
                    LineNumber = 8,
                    Buses = new List<Bus>()
                    {
                        new Bus()
                        {
                            BusNumber = 1,
                            InUse = true,
                        },
                        new Bus()
                        {
                            BusNumber = 2,
                            InUse = true,
                        },
                        new Bus()
                        {
                            BusNumber = 3,
                            InUse = true,
                        }
                    }
                }
            };
            _context.Add(timeTable5);

            TimeTable timeTable6 = new TimeTable()
            {
                Day = "Working days",
                Departures = "12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32 12:32",
                Type = "In City",
                Line = new Line()
                {
                    Name = "8:Liman-Centar-Novo Naselje",
                    LineNumber = 8,
                    Buses = new List<Bus>()
                    {
                        new Bus()
                        {
                            BusNumber = 11,
                            InUse = true,
                        },
                        new Bus()
                        {
                            BusNumber = 12,
                            InUse = true,
                        },
                        new Bus()
                        {
                            BusNumber = 13,
                            InUse = true,
                        }
                    }
                }
            };
            _context.Add(timeTable6);

            if (!_context.PricelistItems.Any())
            {
                UserDiscount ud1 = new UserDiscount()
                {
                    Type = "Student",
                    Value = 20
                };
                _context.Add(ud1);

                UserDiscount ud2 = new UserDiscount()
                {
                    Type = "Regular",
                    Value = 0
                };
                _context.Add(ud1);

                UserDiscount ud3 = new UserDiscount()
                {
                    Type = "Senior",
                    Value = 35
                };
                _context.Add(ud1);


                Item it1 = new Item()
                {
                    Type = "Hourly"
                };
                _context.Add(it1);

                Item it2 = new Item()
                {
                    Type = "Daily"
                };
                _context.Add(it2);


                Item it3 = new Item()
                {
                    Type = "Monthly"
                };
                _context.Add(it3);

                Item it4 = new Item()
                {
                    Type = "Annual"
                };
                _context.Add(it4);
                _context.SaveChanges();

                Pricelist pr = new Pricelist()
                {
                    Active = true,
                    From = DateTime.Now,
                    To = DateTime.Now.AddMonths(4)
                };
                _context.Add(pr);
                _context.SaveChanges();

                PricelistItem prit1 = new PricelistItem()
                {
                    Pricelist = pr,
                    Price = 150,
                    Item = it1
                };
                _context.Add(prit1);

                PricelistItem prit2 = new PricelistItem()
                {
                    Pricelist = pr,
                    Price = 390,
                    Item = it2
                };
                _context.Add(prit2);

                PricelistItem prit3 = new PricelistItem()
                {
                    Pricelist = pr,
                    Price = 3450,
                    Item = it3
                };
                _context.Add(prit3);

                PricelistItem prit4 = new PricelistItem()
                {
                    Pricelist = pr,
                    Price = 12050,
                    Item = it4
                };
                _context.Add(prit4);
            }
            
            _context.SaveChanges();

        }
    }
}