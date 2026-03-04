using ECommerceAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Dodaj CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        builder =>
        {
            builder.WithOrigins(
                "http://localhost:5173",  // Dev lokalnie
                "https://delightful-stone-0b3f82703.1.azurestaticapps.net"  // Azure Static Web Apps
            )
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


builder.Services.AddControllers();

// 2. Skonfiguruj DbContext dla bazy w pamięci
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseInMemoryDatabase("ECommerceDb"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
    dbContext.Database.EnsureCreated();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 3. Użyj polityki CORS
app.UseCors("AllowVueApp"); 

app.UseAuthorization();

app.MapControllers();

app.Run();