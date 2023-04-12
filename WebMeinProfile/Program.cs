using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebMeinProfile.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var conStr = builder.Configuration.GetConnectionString("MyDB");
builder.Services.AddDbContext<MeinProfileContext>(options => options.UseSqlServer(conStr));
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(conStr));


// identity
builder.Services.AddIdentity<AppUser, IdentityRole>(o =>
{
    // setting konfigurasi password dll
    o.Password.RequiredLength = 5;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;

    //o.Lockout.MaxFailedAccessAttempts = 3;
})
.AddEntityFrameworkStores<AppIdentityDbContext>()  // simpan db
.AddDefaultTokenProviders();  // token

// set session expired
builder.Services.ConfigureApplicationCookie(o =>
{
    o.ExpireTimeSpan = TimeSpan.FromMinutes(15); // session 15 menit
    o.SlidingExpiration = true;
});





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
