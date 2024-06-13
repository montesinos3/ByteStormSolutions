using Microsoft.EntityFrameworkCore;
using ByteStormApi.Models;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var URLPermitidas = builder.Configuration.GetSection("AllowedHosts").Value.Split(",");


// Add services to the container.

builder.Services.AddControllers(); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ByteStormContext>((optionsBuilder) =>
            optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("ApiDatabase")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins(URLPermitidas)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});
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

app.Run();
