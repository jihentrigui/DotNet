using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projet.Models;
using Microsoft.Extensions.DependencyInjection;
using Projet.Models.Repository;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services à l'application
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjDBConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDBContext>();

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<ISparePartRepository, SparePartRepository>();


var app = builder.Build();

// Appeler la méthode d'initialisation des rôles et de l'utilisateur responsable
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRolesAndAdminAsync(services);
}

// Configurer le pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Méthode pour ajouter les rôles et l'utilisateur responsable
static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Ajouter les rôles
    var roles = new[] { "Client", "Responsable" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Ajouter l'utilisateur Responsable
    var adminEmail = "responsable@example.com"; // Changez l'adresse e-mail si nécessaire
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var admin = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(admin, "Admin@123"); // Remplacez par un mot de passe sécurisé
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Responsable");
        }
    }
}
