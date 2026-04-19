using Microsoft.AspNetCore.Identity;

namespace EduCobros.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        // ═══ CREAR ROLES ═══
        string[] roles = { "Admin", "Secretaria", "Padre" };
        foreach (var rol in roles)
        {
            if (!await roleManager.RoleExistsAsync(rol))
                await roleManager.CreateAsync(new IdentityRole(rol));
        }

        // ═══ CREAR USUARIO ADMIN ═══
        var adminEmail = "admin@educobros.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        // ═══ CREAR SECRETARIA DE PRUEBA ═══
        var secEmail = "secretaria@educobros.com";
        if (await userManager.FindByEmailAsync(secEmail) == null)
        {
            var sec = new IdentityUser
            {
                UserName = secEmail,
                Email = secEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(sec, "Secret123!");
            await userManager.AddToRoleAsync(sec, "Secretaria");
        }

        // ═══ CREAR PADRE DE PRUEBA ═══
        var padreEmail = "padre@educobros.com";
        if (await userManager.FindByEmailAsync(padreEmail) == null)
        {
            var padre = new IdentityUser
            {
                UserName = padreEmail,
                Email = padreEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(padre, "Padre123!");
            await userManager.AddToRoleAsync(padre, "Padre");
        }
    }
}