using Microsoft.EntityFrameworkCore;
using UserManagement.Api.Services;
using UserManagement.Data;
using UserManagement.Data.Patterns;
using UserManagement.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true);

// Core Services
builder.Services.AddControllers();

// Entity Services
builder.Services.AddDbContext<DbContext, DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IEntityRepository<User>, EntityRepository<User>>();

// Application Services
builder.Services.AddTransient<UserService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors(policy =>
{
    policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
    await dbContext.Database.MigrateAsync();
}

await app.RunAsync();

public partial class Program
{
}
