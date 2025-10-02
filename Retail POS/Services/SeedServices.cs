using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Retail_POS.Models;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Retail_POS.Services
{
    public class SeedServices
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var rolemanager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = scope.ServiceProvider.GetRequiredService<Data.AppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedServices>>();


            try
            {

                logger.LogInformation("Starting database seeding...");
                await context.Database.EnsureCreatedAsync();

                // Seed Roles
                logger.LogInformation("Seeding roles...");
                await AddRoleAsync(rolemanager, "Admin");
                await AddRoleAsync(rolemanager, "User");


                //seed admin user
                logger.LogInformation($"{nameof(SeedServices)}");
                var adminEmail = "admin@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var admin = new ApplicationUser
                    {
                        FullName = "Admin User",
                        UserName = adminEmail,
                        Email = adminEmail,
                        NormalizedUserName = adminEmail.ToUpper(),
                        NormalizedEmail = adminEmail.ToUpper(),
                        EmailConfirmed = false, 
                    };

                    var res = await userManager.CreateAsync(admin, "Admin@123");
                    if (res.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                        logger.LogInformation("Admin user created and assigned to Admin role.");

                    }
                    else
                    {
                        logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", res.Errors.Select(e => e.Description)));

                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
        private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = await roleManager.CreateAsync(new IdentityRole(roleName));

                if (!role.Succeeded)
                {
                    throw new Exception($"Failed to create role '{roleName}': {string.Join(", ", role.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
