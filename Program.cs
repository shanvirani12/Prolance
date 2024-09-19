using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Prolance.Infrastructure.Persistence.Data;
using Prolance.Application.Services;
using Prolance.Domain.Interfaces;
using Prolance.Infrastructure.Persistence.Repositories;
using Prolance.Mapping;
using Prolance.Domain.Entities;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddSession(options =>
{
    // Set session timeout
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust as needed
});


builder.Services.AddAuthorizationBuilder();

//DBContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("OfficeConnection")));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Register Repository and Services
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<AccountService>();

builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddScoped<BidService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Accounts",
    pattern: "{controller=Accounts}/{action=Index}/{id?}");


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Employee" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    string firstName = "admin";
    string lastName = "admin";
    string email = "admin@admin.com";
    string password = "Admin@admin12";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new User();
        user.UserName = email;
        user.Email = email;
        user.FirstName = firstName;
        user.LastName = lastName;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");

    }

}

app.Run();