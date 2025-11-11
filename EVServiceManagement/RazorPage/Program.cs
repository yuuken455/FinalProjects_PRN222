using BLL.IService;
using BLL.Mapping;
using BLL.Service;
using DAL;
using DAL.IRepository;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR(); // thêm

// ========== 1️⃣ Add base services ==========
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// ========== 2️⃣ Database Context ==========
builder.Services.AddDbContext<EVServiceManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========== 3️⃣ AutoMapper ==========
builder.Services.AddAutoMapper(typeof(AutoMappingProfile));

// ========== 4️⃣ Repository & Service DI bindings ==========
// Account
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IAccountService, AccountService>();

// Vehicle
builder.Services.AddScoped<IVehicleRepo, VehicleRepo>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

// Customer & Service
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IServiceRepo, ServiceRepo>();
builder.Services.AddScoped<IServiceService, ServiceService>();

// Appointment
builder.Services.AddScoped<IAppointmentRepo, AppointmentRepo>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// Part & PartRequest (flow Staff ↔ Manager)
builder.Services.AddScoped<IPartRepository, PartRepository>();
builder.Services.AddScoped<IPartService, PartService>();
builder.Services.AddScoped<IPartRequestRepository, PartRequestRepository>();
builder.Services.AddScoped<IPartRequestService, PartRequestService>();
// Report
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
// ========== 5️⃣ Build app ==========
var app = builder.Build();

// ========== 6️⃣ Middleware pipeline ==========
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapHub<BLL.Hubs.NotificationsHub>("/hubs/notify"); // 🔔
// Nếu bạn có xác thực đăng nhập thì bật:
app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
