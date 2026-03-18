// Import Entity Framework Core namespace
// Required to use DbContext and SQL Server connection methods
using Microsoft.EntityFrameworkCore;

// Import your Models namespace
// Required so Program.cs can access EmpContext class
using WebApplication1.Models;

// Import OpenAPI namespace
// Required to use OpenApiInfo class for Swagger documentation
using Microsoft.OpenApi;

namespace WebApplication1
{
    // Main class of the application
    public class Program
    {
        // Entry point of the application
        // Execution starts from here
        public static void Main(string[] args)
        {
            // Create WebApplicationBuilder object
            // This object is used to configure services, database, controllers, etc.
            var builder = WebApplication.CreateBuilder(args);

            // Read connection string from appsettings.json
            // builder.Configuration is ConfigurationManager object
            // GetConnectionString reads value from ConnectionStrings section
            var connectionString =
                builder.Configuration.GetConnectionString("MyConnection")

                // If connection string is null, throw exception
                // Prevents app from running without database connection
                ?? throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' not found."
                );

            // Register EmpContext (DbContext) in Dependency Injection container
            // builder.Services is IServiceCollection object
            // This tells ASP.NET how to create EmpContext object
            builder.Services.AddDbContext<EmpContext>(
                options =>
                    // Configure EmpContext to use SQL Server
                    // connectionString contains database connection info
                    options.UseSqlServer(connectionString)
            );

            // Register controller services
            // Allows ASP.NET to use controller classes
            builder.Services.AddControllers();

            // Register OpenAPI service
            // Used to generate API documentation automatically
            builder.Services.AddOpenApi();

            // Register Swagger generator service
            // Swagger allows testing API in browser
            builder.Services.AddSwaggerGen(c =>
            {
                // Create OpenApiInfo object
                // Defines API documentation details
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "API",   // API title shown in Swagger
                        Version = "v1"  // API version
                    }
                );
            });

            // Build the WebApplication object
            // Converts builder configuration into runnable application
            var app = builder.Build();

            // Check if application is running in Development environment
            if (app.Environment.IsDevelopment())
            {
                // Enable Swagger middleware
                // Generates Swagger JSON endpoint
                app.UseSwagger();

                // Enable Swagger UI middleware
                // Provides web interface to test API
                app.UseSwaggerUI(c =>
                {
                    // Specify Swagger JSON endpoint
                    c.SwaggerEndpoint(
                        "/openapi/v1.json",
                        "My API V1"
                    );

                    // Set Swagger UI URL to /swagger
                    // Example: https://localhost:5001/swagger
                    c.RoutePrefix = "swagger";
                });

                // Map OpenAPI endpoint
                app.MapOpenApi();
            }

            // Enable HTTPS redirection
            // Redirects HTTP requests to HTTPS
            app.UseHttpsRedirection();

            // Enable authorization middleware
            // Used when [Authorize] attribute is applied
            app.UseAuthorization();

            // Map incoming HTTP requests to controller methods
            // Example: GET /api/employee → EmployeeController.Get()
            app.MapControllers();

            // Start the web application
            // Application begins listening for requests
            app.Run();
        }
    }
}
