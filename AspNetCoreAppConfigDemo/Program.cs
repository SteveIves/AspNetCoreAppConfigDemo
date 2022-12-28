
using AspNetCoreAppConfigDemo.Models;
using Microsoft.Extensions.Options;

namespace AspNetCoreAppConfigDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Add a service to expose the ApplicationSettings section from appsettings.json
            // The service will be available from DI via one of three interfaces:
            // - IOptions<ApplicationSettings>          Singleton service, cached for app lifetime
            // - IOptionsSnapshot<ApplicationSettings>  Scoped service, resolved for each HTTP request
            // - IOptionsMonitor<ApplicationSettings>   Singleton service, properties always read the latest value
            // So when using IOptions the app must be restarted for changes to be picked up,
            // but when using IOptionsSnapshot or IOptionsMonitor, changes in setting values will be picked
            // up immediately after the file is saved.
            builder.Services.Configure<ApplicationSettings>(
                builder.Configuration.GetSection(nameof(ApplicationSettings)));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

            // Add a GET endpoint to expose the applicaiton settings
            // This is just so the settings can be visualized for testing
            app.MapGet("settings", (IOptionsMonitor<ApplicationSettings> options) =>
            {
                var response = new
                {
                    options.CurrentValue.ApplicationName,
                    options.CurrentValue.Version
                };
                return Results.Ok(response);
            });

            app.Run();
        }
    }
}