using GtrTraingHr.Data;
using GtrTraingHr.Data.Repository.Implemention;
using GtrTraingHr.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GtrDbContext>(op =>
op.UseSqlServer(builder.Configuration.GetConnectionString("GTHRDB"))
.EnableSensitiveDataLogging()
);
builder.Services.AddTransient<IAttendanceRepo,AttendanceRepo>();
builder.Services.AddTransient<ICompanyRepo,CompanyRepo>();
builder.Services.AddTransient<IDepartmentRepo,DepartmentRepo>();
builder.Services.AddTransient<IDesignationRepo,DesignationRepo>();
builder.Services.AddTransient<IEmployeeRepo,EmployeeRepo>();
builder.Services.AddTransient<ISalaryRepo,SalaryRepo>();
builder.Services.AddTransient<IShiftRepo,ShiftRepo>();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
