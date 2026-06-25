using HRMS.API.Extensions;
using HRMS.API.Middleware;
using HRMS.Core.Postgres;
using HRMS.Core.Postgres.Data;
using HRMS.Shared.Application.Extensions;
using HRMS.Shared.Application.GraphQL;
using HRMS.Shared.Infrastructure.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json.Serialization;
using TodoFeature.Application.DTO;
using DocumentsFeature.Application.DTO;
using PayrollFeature.Application.DTO;
using ExpensesFeature.Application.DTO;
using TeamFeature.Application.DTO;
using HRMS.API.RegisterDependencies;
using AnnouncementsFeature.Application.DTO;
using UserFeature.Application.DTO;
using AttendanceFeature.Application.DTO;
using LeaveFeature.Application.DTO;
using PerformanceFeature.Application.DTO;
using TrainingFeature.Application.DTO;
using RecognitionFeature.Application.DTO;
using HRCopilotFeature.Application.DTO;
using ContributionsFeature.Application.DTO;
using RecruitmentFeature.Application.DTO;
using AnalyticsFeature.Application.DTO;
using OnboardingFeature.Application.DTO;
using AuthFeature.Application;
using AuthFeature.Application.Services;
using AuthFeature.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HRMS.API
{
    public class NoCacheFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.HttpContext.WebSockets.IsWebSocketRequest)
            {
                context.HttpContext.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
                context.HttpContext.Response.Headers["Pragma"] = "no-cache";
                context.HttpContext.Response.Headers["Expires"] = "-1";
            }
        }
        public void OnActionExecuting(ActionExecutingContext context) { }
    }

    public class Startup
    {
        public void Configure(WebApplication app, IWebHostEnvironment env, IConfiguration configuration)
        {
            app.UseForwardedHeaders();
            //app.UseStaticFiles();
            
            DataSeeder.SeedData(app);

            app.UseRouting();
            app.UseRequestTimeouts();
            app.UseCors();
            app.AddMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/health", () => "OK");
                endpoints.MapControllers();
                endpoints.MapGraphQL("/graphql").WithOptions(options =>
                {
                    options.Tool.Enable = true;
                });
            });
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            services.AddControllers()
                   .AddJsonOptions(options =>
                   {
                       options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                   });

            services.AddEndpointsApiExplorer();
            services.AddHttpClient();
            
            services.AddInjectionApplication(configuration, [
                typeof(CreateTodoHandler).Assembly,
                typeof(CreateDocumentHandler).Assembly,
                typeof(CreatePayrollHandler).Assembly,
                typeof(CreateExpenseHandler).Assembly,
                typeof(CreateTeamMemberHandler).Assembly,
                typeof(CreateAnnouncementHandler).Assembly,
                typeof(CreateEmployeeHandler).Assembly,
                typeof(CreateAttendanceHandler).Assembly,
                typeof(CreateLeaveHandler).Assembly,
                typeof(CreateGoalHandler).Assembly,
                typeof(CreateTrainingModuleHandler).Assembly,
                typeof(CreateRecognitionHandler).Assembly,
                typeof(ChatHandler).Assembly,
                typeof(CreateContributionHandler).Assembly,
                typeof(CreateJobPostingHandler).Assembly,
                typeof(GenerateReportHandler).Assembly,
                typeof(CreateOnboardingTaskHandler).Assembly,
                typeof(AuthHandler).Assembly
            ]);

            services.AddInjectionPostgres(configuration);
            
            // Register Auth Services
            services.AddScoped<IJwtService, JwtService>();

            // Configure JWT Authentication
            var jwtKey = configuration["Jwt:Key"] ?? "super_secret_key_that_is_at_least_32_characters_long";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
            services.AddModulesDependencyInjection(configuration);
            services.ConfigureApiBehavior();
            services.ConfigureCorsPolicy(configuration);
            services.ConfigureGraphQL(configuration);
            services.AddMemoryCache();
            services.AddMvc(options => { options.Filters.Add(new NoCacheFilter()); });
        }
    }
}
