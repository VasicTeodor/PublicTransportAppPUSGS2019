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
            _context.Database.EnsureCreated();
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
                        Email = $"user{i}@gmail.com",
                        UserType = "Regular",
                        Address = new Address()
                        {
                            City = "Grad",
                            Street = "Ulica",
                            Number = "Broj"
                        },
                        Verified = false,
                        AccountStatus = "Pending activation"
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
                    _userManager.AddToRoleAsync(adm, "Admin").Wait();
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
            if (!_context.Stations.Any())
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

                _context.Add(station);
            }

            if (!_context.TimeTables.Any())
            {
                TimeTable timeTable1 = new TimeTable()
                {
                    Day = "Working days",
                    Departures = "00:12-22-25-34-40-45-58/01:30/02:30/03:30/04:30/05:30/06:00-20-40/07:03-12-23-30-45-58/08:13-22-25-30-45-58/09:03-12-23-30-45-58/10:03-12-23-30-45-58/11:03-12-23-30-45-58/12:03-12-23-30-45-58/13:03-12-23-30-45-58/14:03-12-23-30-45-58/15:03-12-23-30-45-58/16:03-12-23-30-45-58/17:00-23-30-40-58/18:00-23-30-40-58/19:00-23-30-40-58/20:00-23-40-58/21:00-23-40-58/22:00-23-40-58/23:00-30-58",
                    Type = "In City",
                    Line = new Line()
                    {
                        Name = "STANICA-LUTRIJA-LIMAN",
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
                    Departures = "00:12-25-34-40-45-58/01:30/02:30/03:30/04:30/05:30/06:00-25-40/07:03-14-23-37-45-52/08:13-22-25-30-45-58/09:03-12-30-45-58/10:03-12-27-30-41-58/11:03-12-23-30-45-58/12:03-12-23-30-45-58/13:03-12-23-30-45-58/14:03-12-23-30-45-58/15:03-12-23-30-45-58/16:03-12-23-30-45-58/17:00-23-30-40-58/18:00-23-30-40-58/19:00-23-30-40-58/20:00-23-40-58/21:00-23-40-58/22:00-23-40-58/23:00-30-58",
                    Type = "In City",
                    Line = new Line()
                    {
                        Name = "LIMAN-LUTRIJA-STANICA",
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
                    Departures = "00:12-22-25-34-40-45-58/01:30/02:30/03:30/04:30/05:30/06:00-20-40/07:03-12-23-30-45-58/08:13-22-25-30-45-58/09:03-12-23-30-45-58/10:03-12-23-30-45-58/11:03-12-23-30-45-58/12:03-12-23-30-45-58/13:03-12-23-30-45-58/14:03-12-23-30-45-58/15:03-12-23-30-45-58/16:03-12-23-30-45-58/17:00-23-30-40-58/18:00-23-30-40-58/19:00-23-30-40-58/20:00-23-40-58/21:00-23-40-58/22:00-23-40-58/23:00-30-58",
                    Type = "In City",
                    Line = new Line()
                    {
                        Name = "CENTAR-TELEP",
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
                    Departures = "00:12-22-25-34-40-45-58/01:30/02:30/03:30/04:30/05:30/06:00-20-40/07:03-12-23-30-45-58/08:13-22-25-30-45-58/09:03-12-23-30-45-58/10:03-12-23-30-45-58/11:03-12-23-30-45-58/12:03-12-23-30-45-58/13:03-12-23-30-45-58/14:03-12-23-30-45-58/15:03-12-23-30-45-58/16:03-12-23-30-45-58/17:00-23-30-40-58/18:00-23-30-40-58/19:00-23-30-40-58/20:00-23-40-58/21:00-23-40-58/22:00-23-40-58/23:00-30-58",
                    Type = "In City",
                    Line = new Line()
                    {
                        Name = "TELEP-CENTAR",
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
                    Departures = "00:12-22-25-34-40-45-58/01:30/02:30/03:30/04:30/05:30/06:00-20-40/07:03-12-23-30-45-58/08:13-22-25-30-45-58/09:03-12-23-30-45-58/10:03-12-23-30-45-58/11:03-12-23-30-45-58/12:03-12-23-30-45-58/13:03-12-23-30-45-58/14:03-12-23-30-45-58/15:03-12-23-30-45-58/16:03-12-23-30-45-58/17:00-23-30-40-58/18:00-23-30-40-58/19:00-23-30-40-58/20:00-23-40-58/21:00-23-40-58/22:00-23-40-58/23:00-30-58",
                    Type = "In City",
                    Line = new Line()
                    {
                        Name = "NOVO NASELJE-CENTAR-LIMAN",
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
                    Departures = "00:12-22-25-34-40-45-58/01:30/02:30/03:30/04:30/05:30/06:00-20-40/07:03-12-23-30-45-58/08:13-22-25-30-45-58/09:03-12-23-30-45-58/10:03-12-23-30-45-58/11:03-12-23-30-45-58/12:03-12-23-30-45-58/13:03-12-23-30-45-58/14:03-12-23-30-45-58/15:03-12-23-30-45-58/16:03-12-23-30-45-58/17:00-23-30-40-58/18:00-23-30-40-58/19:00-23-30-40-58/20:00-23-40-58/21:00-23-40-58/22:00-23-40-58/23:00-30-58",
                    Type = "In City",
                    Line = new Line()
                    {
                        Name = "LIMAN-CENTAR-NOVO NASELJE",
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
            }

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
                _context.Add(ud2);

                UserDiscount ud3 = new UserDiscount()
                {
                    Type = "Senior",
                    Value = 35
                };
                _context.Add(ud3);


                Item it1 = new Item()
                {
                    Type = "Hourly",
                    Description = "This ticket is valid just for one hour"
                };
                _context.Add(it1);

                Item it2 = new Item()
                {
                    Type = "Daily",
                    Description = "This ticket is valid until 00:00 next day"
                };
                _context.Add(it2);


                Item it3 = new Item()
                {
                    Type = "Monthly",
                    Description = "This ticket is valid for one month"
                };
                _context.Add(it3);

                Item it4 = new Item()
                {
                    Type = "Annual",
                    Description = "This ticket is valid for one year"
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