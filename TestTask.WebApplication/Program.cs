using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Interface;
using TestTask.Repository;
using TestTask.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddScoped<IUserContact, UserContactRepo>();
builder.Services.AddScoped<IImage, ImageSer>();


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
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
