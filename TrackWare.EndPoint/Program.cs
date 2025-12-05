
using System.Data;
using TrackWare.Application.Interfaces;
using TrackWare.Application.UseCases;
using TrackWare.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Connection string
var connectionStr = builder.Configuration.GetConnectionString("connStr");
//connectionStr = "Password=123;Persist Security Info=True;User ID=sa;Initial Catalog=TrackWare;Data Source=DESKTOP-VP05SP7\\SQLEXPRESS01";
Console.WriteLine($"Connection String from Program.cs: '{connectionStr}'");
var connObj=new  SqlConnection(connectionStr.Trim());
builder.Services.AddScoped<IDbConnection>(sp => connObj);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserLoginHandler,UserLoginHandler>();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // <-- MUST be before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
