using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ProductsEndpoint;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductDbContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr"), o =>
                    {
                        o.EnableRetryOnFailure(
                           maxRetryCount: 5,
                           maxRetryDelay: TimeSpan.FromSeconds(10),
                           errorNumbersToAdd: null
                            );
                    });
                }
            );

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");


app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

app.MapGet("/products", async (ProductDbContext db, string? searchText) =>
{
    try
    {
        var query = db.Products.AsQueryable();

        if (!string.IsNullOrEmpty(searchText))
        {
            query = query.Where(p => p.Name.Contains(searchText));
        }
        var products = await query.ToListAsync();
        return Results.Ok(new
        {
            totalProducts = products.Count,
            foundProducts = products,
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(ex.Message);
    }
});


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
