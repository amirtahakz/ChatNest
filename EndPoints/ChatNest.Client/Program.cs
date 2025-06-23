using ChatNest.Client.Hubs;
using ChatNest.Core.Services.Chats;
using ChatNest.Core.Services.Chats.ChatGroups;
using ChatNest.Core.Services.Roles;
using ChatNest.Core.Services.Users;
using ChatNest.Core.Services.Users.UserGroups;
using ChatNest.Data.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


services.AddControllersWithViews();

// For Entity Framework  
services.AddDbContext<ApplicationDbContext>(option => { option.UseSqlServer(connectionString); });


services.AddScoped<IChatGroupService, ChatGroupService>();
services.AddScoped<IChatService, ChatService>();
services.AddScoped<IRoleService, RoleService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IUserGroupService, UserGroupService>();

services.AddSignalR();

services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(option =>
{
    option.LoginPath = "/auth";
    option.LogoutPath = "/auth/Logout";
    option.ExpireTimeSpan = TimeSpan.FromDays(30);
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");
});

app.Run();
