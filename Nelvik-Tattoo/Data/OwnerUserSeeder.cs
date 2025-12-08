using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Nelvik_Tattoo.Data
{
    public static class OwnerUserSeeder
    {
        // Her definerer vi eier-brukeren
        private const string OwnerEmail = "owner@nelviktattoo.no";
        private const string OwnerPassword = "Owner!234";

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;;
            
            var db = provider.GetRequiredService<ApplicationDbContext>();
            await db.Database.EnsureCreatedAsync();
            
            var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();

            // Finnes brukeren allerede?
            var owner = await userManager.FindByEmailAsync(OwnerEmail);

            if (owner == null)
            {
                // Oppretter eier-bruker
                owner = new IdentityUser
                {
                    UserName = OwnerEmail,
                    Email = OwnerEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(owner, OwnerPassword);
            }
        }
    }
}