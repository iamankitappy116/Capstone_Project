using AuthenticationExample.Data;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var con = builder.Configuration.GetConnectionString("PharmacyConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<PharmacyContext>(options =>
    options.UseSqlServer(con));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // Enable Swagger UI middleware
    // Provides web interface to test API
    app.UseSwaggerUI(c =>
    {
        // Specify Swagger JSON endpoint
        c.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "My API V1"
        );

        // Set Swagger UI URL to /swagger
        // Example: https://localhost:5001/swagger
        c.RoutePrefix = "swagger";
    });
    app.UseMigrationsEndPoint();
    // Map OpenAPI endpoint
    app.MapOpenApi();
}
else
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
app.MapRazorPages();

app.Run();