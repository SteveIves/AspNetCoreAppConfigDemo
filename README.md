# ASP .NET Core AppConfig Demo

This repository contains an example of how to implement ASP.NET Core application settings using the
options pattern. The content is based on the following YouTube tutorial:

https://youtu.be/wxYt0motww0

To implement this functionality:

1. Add an ApplicationSettings section to the appsettings.json file. For example:

   ```
   {
      "Logging": {
         "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
      }
   },
      "AllowedHosts": "*",
      "ApplicationSettings": {
         "ApplicationName": "ASP.NET Core AppConfig Demo",
         "Version": "1.0"
      }
   }
   ```

2. Add an ApplicationSettings model class. For example:

   ```
   namespace AspNetCoreAppConfigDemo.Models
   {
      public class ApplicationSettings
      {
         public string ApplicationName { get; init; } = String.Empty;
         public string Version { get; init; } = String.Empty;
      }
   }
   ```

3. Add a service to expose the ApplicationSettings section from appsettings.json

   The service will be available from DI via one of three interfaces:

   * IOptions<ApplicationSettings>
     * A singleton service, cached for app lifetime

   * IOptionsSnapshot<ApplicationSettings>
     * A scoped service, resolved for each HTTP request

   * IOptionsMonitor<ApplicationSettings>
     * A singleton service, properties always read the latest value

   When using IOptions the app must be restarted for changes to be picked up,
   but when using IOptionsSnapshot or IOptionsMonitor, changes in setting values
   will be picked up immediately after the file is saved.

   For example:

   ```
   builder.Services.Configure<ApplicationSettings>(
      builder.Configuration.GetSection(nameof(ApplicationSettings)));
   ```

4. Optionally, add an endpoint to expose the application settings. For example:

   ```
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
   ```
