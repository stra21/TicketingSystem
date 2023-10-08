using BL.Interfaces;
using BL.Services;
using DL.Interfaces;
using DL.Models;
using DL.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use the default naming policy (CamelCasePropertyNamingPolicy)
    options.JsonSerializerOptions.WriteIndented = true; // Pretty-print JSON responses
                                                        // Add any additional configuration options here
});
builder.Services.AddScoped<ICallsService, CallsService>();
builder.Services.AddScoped<IModulesService, ModulesService>();
builder.Services.AddScoped<ICallTypesService, CallTypesService>();
builder.Services.AddScoped<IUploadsService, UploadsService>();
builder.Services.AddScoped<IUsersService,UserService>();
builder.Services.AddScoped<ICommentsService,CommentsService>();
builder.Services.AddScoped<ChangesIntercepor, ChangesIntercepor>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(m =>
{
    m.LoginPath = new PathString("/Login/Login");

});
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
DL.Utilities.AppSettings.ConnectionString = builder.Configuration.GetSection("ConnectionStrings:DefaultConnection").Get<string>();
builder.Services.AddDbContext<DL.Models.TicketingSystemContext>();
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
