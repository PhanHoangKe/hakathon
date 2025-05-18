using System;
using System.Net.Http;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using hakathon.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
// using hakathon.Services;
// using hakathon.Services.Email;
// using hakathon.Services.Vnpay;
// using hakathon.Services.AI;
// using hakathon.Services.OpenAI;

var builder = WebApplication.CreateBuilder(args);


// ========================== CẤU HÌNH SERVICES ========================== //

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Kết nối CSDL SQL Server
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connection))
{
    throw new InvalidOperationException("Chuỗi kết nối 'DefaultConnection' chưa được cấu hình.");
}

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(connection);
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment()); // Chỉ bật khi phát triển
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Controllers + Views + JSON config
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Swagger / API explorer (nếu cần)
builder.Services.AddEndpointsApiExplorer();

// Xác thực Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.Cookie.Name = "hakathonAuth";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });

// ========== CÁC DỊCH VỤ CUSTOM ========== //
// <--- ĐÂY LÀ NƠI BẠN THƯỜNG ĐẶT CẤU HÌNH OPTIONS --->
builder.Services.Configure<Email>(builder.Configuration.GetSection("Emails"));
// Program.cs

// builder.Services.AddScoped<IVnPayService, VnPayService>();
// builder.Services.AddScoped<IEmailService, EmailService>();
// builder.Services.AddHostedService<ExpiredCourseChecker>();

// // ChatGPT (OpenAI)
// builder.Services.Configure<OpenAIOptions>(
//     builder.Configuration.GetSection("OpenAI"));
// builder.Services.AddHttpClient<IOpenAIService, OpenAIService>();

// Ollama - Cấu hình từ section trong appsettings.json
// builder.Services.Configure<OllamaOptions>(builder.Configuration.GetSection("Ollama"));
// builder.Services.AddHttpClient<IAIService, OllamaService>();
// builder.Services.AddScoped<IAIService, OllamaService>();

// ========================== CẤU HÌNH MIDDLEWARE PIPELINE ========================== //

var app = builder.Build();

// Xử lý lỗi
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// Cấu hình MIME types mở rộng (AVIF)
var contentTypeProvider = new FileExtensionContentTypeProvider();
contentTypeProvider.Mappings[".avif"] = "image/avif";

// Static files gốc
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = contentTypeProvider
});

//Static files từ thư mục "uploads"
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/files",
    ContentTypeProvider = contentTypeProvider
});

// Middleware thứ tự
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// ========================== ĐỊNH TUYẾN ========================== //

// API Controller
app.MapControllers();

// Area (Admin, Teacher,...)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// // Route riêng cho khóa học
// app.MapControllerRoute(
//     name: "courses",
//     pattern: "Home/Courses/{id:int}",
//     defaults: new { controller = "Home", action = "Courses" }
// );

app.Run();