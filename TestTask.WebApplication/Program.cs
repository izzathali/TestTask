using Microsoft.EntityFrameworkCore;
using TestTask.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Db Connection
var connectionString = builder.Configuration.GetConnectionString("DevConnection");

builder.Services.AddDbContext<TestTaskDbContext>(option =>
option.UseSqlServer(connectionString), ServiceLifetime.Transient);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
