using Microsoft.AspNetCore.Identity;
using SimpleShop.Models.Entities;

namespace SimpleShop.Data.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider)
    {
        var roleManager =
            serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager =
            serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();



        string[] roles =
        {
            "Admin",
            "Customer"
        };



        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(role));
            }
        }



        var adminEmail = "admin@simpleshop.com";

        var adminUser =
            await userManager.FindByEmailAsync(adminEmail);



        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };



            var result =
                await userManager.CreateAsync(
                    adminUser,
                    "Admin123!");



            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    adminUser,
                    "Admin");
            }
        }
    }
}