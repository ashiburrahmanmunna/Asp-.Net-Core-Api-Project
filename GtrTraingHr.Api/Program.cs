using GtrTraingHr.Data.Repository.Implemention;
using GtrTraingHr.Data.Repository.Interface;
using GtrTraingHr.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<GtrDbContext>(op =>
op.UseSqlServer(builder.Configuration.GetConnectionString("GTHRDB"))
.EnableSensitiveDataLogging()
);
builder.Services.AddTransient<IAttendanceRepo, AttendanceRepo>();
builder.Services.AddTransient<ICompanyRepo, CompanyRepo>();
builder.Services.AddTransient<IDepartmentRepo, DepartmentRepo>();
builder.Services.AddTransient<IDesignationRepo, DesignationRepo>();
builder.Services.AddTransient<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddTransient<ISalaryRepo, SalaryRepo>();
builder.Services.AddTransient<IShiftRepo, ShiftRepo>();
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
