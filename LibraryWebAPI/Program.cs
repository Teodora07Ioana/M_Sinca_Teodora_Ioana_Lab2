using Microsoft.EntityFrameworkCore;
using M_Sinca_Teodora_Ioana_Lab2;
using Microsoft.Extensions.DependencyInjection;
using LibraryWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryWebAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("M_Sinca_Teodora_Ioana_Lab2Context") ?? throw new InvalidOperationException("Connection string 'LibraryWebAPIContext' not found.")));



// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddDbContext<M_Sinca_Teodora_Ioana_Lab2Context>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
