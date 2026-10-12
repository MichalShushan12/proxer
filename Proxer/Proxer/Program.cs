using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Context;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Proxer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. חיבור לבסיס הנתונים
            builder.Services.AddDbContext<ProxerContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ProxerContext"));
            });

            // 2. הזרקת השירותים (Dependency Injection) - חייב להיות לפני Build()
            builder.Services.AddScoped<IRefactorLogService, RefactorLogService>();
            builder.Services.AddScoped<IRefactorLogRepository, RefactorLogRepository>();
            builder.Services.AddScoped<IAIService, AIService>();

            // 3. הגדרת Rate Limiter (חייב להיות לפני Build)
            builder.Services.AddRateLimiter(o =>
            {
                o.RejectionStatusCode = 429;
                o.AddFixedWindowLimiter("api", opt =>
                {
                    opt.PermitLimit = 20;
                    opt.Window = TimeSpan.FromMinutes(1);
                });
            });

            // 4. הגדרת פוליסי של CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // 5. סדר ה-Middleware הנכון
            app.UseCors("AllowReactApp");

            app.UseRateLimiter();

            app.MapControllers();

            app.Run();


        }
    }
}