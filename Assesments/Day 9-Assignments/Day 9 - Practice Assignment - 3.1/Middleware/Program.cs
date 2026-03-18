var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Enforce HTTPS
app.UseHttpsRedirection();

// Custom Logging Middleware
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
    Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");
});

// Global Error Handling Middleware
app.UseExceptionHandler("/error.html");

// Content Security Policy
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy",
        "default-src 'self'; script-src 'self'; style-src 'self'");
    await next();
});

// Enable Static Files
app.UseStaticFiles();

app.MapGet("/", () => "Middleware Application Running");

app.Run();