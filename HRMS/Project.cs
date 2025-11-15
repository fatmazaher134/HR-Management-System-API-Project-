using HRMS.DependencyInjection;
using Microsoft.OpenApi;

namespace HRMS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            // Add custom services
            builder.Services.AddDataAccessServices(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(Program).Assembly);


            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "HRMS",
                    Description = "An ASP.NET Core Web API for..."
                });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    await HRMS.Data.RoleSeeder.SeedRolesAsync(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding roles.");
                }
            }
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "HRMS API V1");
                    options.RoutePrefix = string.Empty; // ⬅️ Makes Swagger the default page
                });

            }

            app.MapGet("/", () => Results.Redirect("/index.html"));

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();


            app.Run();
        }
    }
}