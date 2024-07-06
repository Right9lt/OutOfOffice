using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Context;
using OutOfOffice.DTO.ApprovalRequest;
using OutOfOffice.DTO.Employee;
using OutOfOffice.DTO.LeaveRequest;
using OutOfOffice.DTO.Project;
using OutOfOffice.Entities;
using OutOfOffice.Helpers;
using OutOfOffice.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var _config = builder.Configuration;


builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<Employee, EmployeeOutDTO>();
    cfg.CreateMap<Employee, EmployeeEditDTO>()
        .ForMember(dest => dest.Password, opt => opt.MapFrom(src => ""));
    cfg.CreateMap<Employee, PeoplePartnerDTO>();
    cfg.CreateMap<Employee, EmployeeSelectionListDTO>();
    cfg.CreateMap<Project, ProjectOutDTO>().MaxDepth(4);
    cfg.CreateMap<Project, ProjectEditDTO>();
    cfg.CreateMap<LeaveRequest, LeaveRequestOutDTO>();
    cfg.CreateMap<ApprovalRequest, ApprovalRequestOutDTO>();
});

builder.Services.AddDbContext<OutOfOfficeContext>(options =>
    {
    options.UseNpgsql(_config["ConnectionStrings:DB"]);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => 
        {
            options.LoginPath = new PathString("/Auth/Login");
            options.AccessDeniedPath = new PathString("/Home/");
            
        });

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<LeaveRequestService>();
builder.Services.AddScoped<ApprovalRequestService>();



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
