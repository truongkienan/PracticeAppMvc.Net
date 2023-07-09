using Microsoft.EntityFrameworkCore;
using PracticeAppMvc.Net.Models;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<PracticeAppMvcNetContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("PracticeAppMvcNetContext") ?? throw new InvalidOperationException("Connection string 'PracticeAppMvcNetContext' not found.")));

// Add services to the container.
//builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppMvcConnectionString"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
//app.MapAreaControllerRoute(
//                 name: "dashboard",
//                 areaName: "Dashboard",
//                 pattern: "Dashboard/{controller=Home}/{action=Index}");
app.MapControllerRoute(name: "dashboard", pattern: "{area:exists}/{controller=home}/{action=index}/{id?}");

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
