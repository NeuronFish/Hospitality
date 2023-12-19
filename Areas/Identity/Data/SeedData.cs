#nullable disable

using Hospitality.Data;
using Hospitality.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospitality.Areas.Identity.Data
{
    public static class SeedData
    {
        public static bool IsInitialized(IServiceProvider serviceProvider)
        {
            using (var context = new HospitalityContext(
                serviceProvider.GetRequiredService<DbContextOptions<HospitalityContext>>()))
            {
                context.Database.EnsureCreated();
                if (context.Users.Any())
                    return true; // DB has been seeded
                else
                    return false;
            }
        }
        public static async Task Initialize(IServiceProvider serviceProvider, string[] usersPW)
        {
            using (var context = new HospitalityContext(
                serviceProvider.GetRequiredService<DbContextOptions<HospitalityContext>>()))
            {
                await EnsureRoles(serviceProvider);
                int hostelId = EnsureHostel(context);
                string chiefId = await SeedDb(serviceProvider, usersPW, hostelId);
                SetChief(context, hostelId, chiefId);
             }
        }
        private static async Task EnsureRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            if (roleManager.FindByNameAsync("Manager").Result == null)
                await roleManager.CreateAsync(new IdentityRole("Manager"));
            if (roleManager.FindByNameAsync("Employee").Result == null)
                await roleManager.CreateAsync(new IdentityRole("Employee"));
        }
        private static int EnsureHostel(HospitalityContext context)
        {
            if (!context.Hostels.Any())
            {
                context.Hostels.Add(new Hostel
                {
                    Name = "Star"
                });
                context.SaveChanges();
                int id = context.Hostels.First().Id;
                context.Floors.AddRange(new List<Floor>()
                {
                    new Floor()
                    {
                        HostelId = id,
                        Index = 1
                    },
                    new Floor()
                    {
                        HostelId = id,
                        Index = 2
                    }
                });
                context.SaveChanges();
                id = context.Floors.First(f => f.Index == 1).Id;
                context.Rooms.AddRange(new List<Room>()
                {
                    new Room()
                    {
                        FloorId = id,
                        Index = 1,
                        Name = "№111",
                        Sleepplaces = 3,
                        SleepDesc = "Двоспальне та одномісне ліжка",
                        Description = "Кімната з телевізором та тумбочкою",
                        Price = 170
                    },
                    new Room()
                    {
                        FloorId = id,
                        Index = 2,
                        Name = "№112",
                        Sleepplaces = 2,
                        SleepDesc = "Два одномістні ліжка",
                        Description = "Кімната з тумбочкою",
                        Price = 125
                    },
                    new Room()
                    {
                        FloorId = id,
                        Index = 3,
                        Name = "№113",
                        Sleepplaces = 1,
                        SleepDesc = "Одномістне ліжко",
                        Description = "Кімната з тумбочкою",
                        Price = 100
                    }
                });
                id = context.Floors.First(f => f.Index == 2).Id;
                context.Rooms.AddRange(new List<Room>()
                {
                    new Room()
                    {
                        FloorId = id,
                        Index = 1,
                        Name = "№211",
                        Sleepplaces = 2,
                        SleepDesc = "Двухспальне ліжко",
                        Description = "Кімната з телевізором та тумбочкою",
                        Price = 150
                    },
                    new Room()
                    {
                        FloorId = id,
                        Index = 2,
                        Name = "№212",
                        Sleepplaces = 1,
                        SleepDesc = "Одномістне ліжко",
                        Description = "Кімната з телевізором та тумбочкою",
                        Price = 125
                    }
                });
                context.SaveChanges();
            }
            return context.Hostels.First().Id;
        }
        private static async Task<string> SeedDb(IServiceProvider serviceProvider, string[] usersPW,
            int hostelId)
        {
            var userManager = serviceProvider.GetService<UserManager<UserIdent>>();
            UserIdent[] users = new UserIdent[] {
                new UserIdent
                {
                    UserName = "AndKashtan@gmail.com",
                    Name = "Андрій",
                    Surname = "Каштанов",
                    PlaceId = hostelId,
                    EmailConfirmed = true
                },
                new UserIdent
                {
                    UserName = "IvanBugr@gmail.com",
                    Name = "Іван",
                    Surname = "Багряний",
                    PlaceId = hostelId,
                    EmailConfirmed = true
                },
                new UserIdent
                {
                    UserName = "ArtemVsevol@gmail.com",
                    Name = "Артем",
                    Surname = "Всеволод",
                    PlaceId = hostelId,
                    EmailConfirmed = true
                }
            };
            string[] roles = new string[] { "Manager", "Employee", "Employee" };
            for (int i = 0; i < users.Length; i++)
            {
                await userManager.CreateAsync(users[i], usersPW[i]);
                await userManager.AddToRoleAsync(users[i], roles[i]);
            }
            return users[0].Id;
        }
        private static void SetChief(HospitalityContext context, int hostelId, string chiefId)
        {
            context.Hostels.First(h => h.Id == hostelId).ChiefId = chiefId;
            context.SaveChanges();
        }

    }
}
