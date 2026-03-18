using BookStoreAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>{
       options.JsonSerializerOptions.PropertyNamingPolicy =
       System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
                     ?? "Data Source=bookstore.db"));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "BookStore API",
        Version     = "v1",
        Description = "A RESTful API for managing books and authors. " +
                      "Supports full CRUD on both resources, plus nested routing " +
                      "to retrieve all books by a given author."
    });

    
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookStoreContext>();
    db.Database.EnsureCreated(); // Creates schema + seed data if the DB doesn't exist yet
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
