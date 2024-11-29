using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using M_Sinca_Teodora_Ioana_Lab2;
using M_Sinca_Teodora_Ioana_Lab2.Data;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<MyLibraryContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("M_Sinca_Teodora_Ioana_Lab2Context") ?? throw new InvalidOperationException("Connection string 'M_Sinca_Teodora_Ioana_Lab2Context' not found.")));


builder.Services.AddDbContext<MyLibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("M_Sinca_Teodora_Ioana_Lab2Context") ?? throw new InvalidOperationException("Connection string 'LibraryWebAPIContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Initialize(services);
}


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
