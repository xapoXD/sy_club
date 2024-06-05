using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using club.soundyard.web.Data;
using club.soundyard.web.Models;
using club.soundyard.web.Services;
using IMailService = club.soundyard.web.Services.IMailService;
using MailService = club.soundyard.web.Services.MailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddControllersWithViews().AddNToastNotifyToastr();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseNToastNotify();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();



//for demonstration purposes
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

    var roles = new[] { "Admin", "Manager", "Member", "Visitor" };
    string agreement = "";
    foreach (var role in roles)
    {
        if(!await roleManager.RoleExistsAsync(role))
        {
            if(role == "Admin")
            {
                agreement = "Custom property - Admin Agreement!";
                await roleManager.CreateAsync(new ApplicationRole(role, agreement));
            }
            else if(role == "Manager")
            {
                agreement = "Custom property - Manager Agreement!";
                await roleManager.CreateAsync(new ApplicationRole(role, agreement));
            }
            else if(role == "Member")
            {
                agreement = "Custom property - Memeber Agreement!";
                await roleManager.CreateAsync(new ApplicationRole(role, agreement));
            }
            else if (role == "Visitor")
            {
                agreement = "Custom property - Visitor Agreement!";
                await roleManager.CreateAsync(new ApplicationRole(role, agreement));
            }
        }

    }
}


//for demonstration purposes
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string email = "admin@admin.com";
    string password = "Password123!";

    string email2 = "manager@manager.com";
    string password2 = "Password123!";


    if (await userManager.FindByEmailAsync(email) == null){
        var user = new IdentityUser();
        user.Email = email;
        user.UserName = email;
        user.EmailConfirmed = true;
     
        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");
    }


    if (await userManager.FindByEmailAsync(email2) == null)
    {
        var user2 = new IdentityUser();
        user2.Email = email2;
        user2.UserName = email2;
        user2.EmailConfirmed = true;

        await userManager.CreateAsync(user2, password2);

        await userManager.AddToRoleAsync(user2, "Manager");
    }


}



app.Run();
