using BackendRedo.Models;
using BackendRedo.Services;
using BackendRedo.Services.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<FormService>();

var connectionString = builder.Configuration.GetConnectionString("RedoBackend");

builder.Services.AddCors(options => options.AddPolicy("RedoPolicy", 
builder => {
    builder.WithOrigins("http://localhost:5120", "https://nextjsforms.vercel.app", "http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod();
}
));

builder.Services.AddDbContext<DataContext>(Options => Options.UseSqlServer(connectionString));

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

//app.UseHttpsRedirection();

app.UseCors("RedoPolicy"); 

app.UseAuthorization();

app.MapControllers();

app.Run();
