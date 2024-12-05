using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using M_Sinca_Teodora_Ioana_Lab2;
using M_Sinca_Teodora_Ioana_Lab2.Data;
using M_Sinca_Teodora_Ioana_Lab2.Hubs;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyLibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("M_Sinca_Teodora_Ioana_Lab2Context")));

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
});

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("OnlySales", policy =>
    {
        policy.RequireRole("Manager");
        policy.RequireClaim("Department", "Sales");
    });
    opts.AddPolicy("SalesManager", policy =>
    {
        policy.RequireRole("Manager");
       
    });
});

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Initialize(services);
}

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

app.MapHub<ChatHub>("/Chat");
app.MapHub<NotificationHub>("/Notification");
app.MapRazorPages();

app.Run();
