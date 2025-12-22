using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Nelvik_Tattoo.Data
{
    public static class OwnerUserSeeder
    {
        private const string OwnerEmail = "owner@nelviktattoo.no";
        private const string OwnerPassword = "Owner!234"; // Bytt til noe sikkert

        public const string OwnerRole = "Owner";
        public const string StaffRole = "Staff";

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            var db = provider.GetRequiredService<ApplicationDbContext>();
            await db.Database.EnsureCreatedAsync();

            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in new[] { OwnerRole, StaffRole })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                        throw new Exception($"Kunne ikke opprette rolle '{role}': " +
                                            string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }

            var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();

            var owner = await userManager.FindByEmailAsync(OwnerEmail);
            if (owner == null)
            {
                owner = new IdentityUser
                {
                    UserName = OwnerEmail,
                    Email = OwnerEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(owner, OwnerPassword);
                if (!result.Succeeded)
                    throw new Exception("Kunne ikke opprette owner: " +
                                        string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (!await userManager.IsInRoleAsync(owner, OwnerRole))
            {
                var addRoleResult = await userManager.AddToRoleAsync(owner, OwnerRole);
                if (!addRoleResult.Succeeded)
                    throw new Exception("Kunne ikke gi Owner-rolle: " +
                                        string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
            }
        }
    }
}
