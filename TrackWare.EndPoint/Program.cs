
using System.Data;
using TrackWare.Application.Interfaces;
using TrackWare.Application.UseCases;
using TrackWare.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Data.SqlClient;
using Microsoft.OpenApi.Models;
using TrackWare.Infrastructure.DataProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Add JWT Authentication Support
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT token like: Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



var connectionStr = builder.Configuration.GetConnectionString("connStr");

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(connectionStr.Trim())
);


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ICompanyProfileRepositor, CompanyProfileRepository>();
builder.Services.AddScoped<IUserLoginHandler,UserLoginHandler>();
builder.Services.AddScoped<ICompanyInfoHandle, CompanyInfoHandle>();
builder.Services.AddScoped<IUserMenuHandler, UserMenuHandler>();

builder.Services.AddScoped<IListOptionsRepository, ListOptionsRepository>();
builder.Services.AddScoped<IColumnSchemaProvider, GridLayoutProvider>();
 
builder.Services.AddScoped<IGridDataProvider, StoredProcGridProvider>();
builder.Services.AddScoped<IListHandler, ListHandler>();


builder.Services.AddScoped<IListDataHandler, ListDataHandler>();

builder.Services.AddScoped<ICrudPermissionRepository, CrudPermissionRepository>();
builder.Services.AddScoped<ICrudDataRepository, CrudDataRepository>();
builder.Services.AddScoped<ILookupRepository, LookupRepository>();

 builder.Services.AddScoped<ICrudDataResolver, CRUDDataResolver>();
builder.Services.AddScoped<ICrudDataSaver, CRUDDataSaver>();
 
builder.Services.AddScoped<ICRUDHelper, CRUDHelper>();




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

var app = builder.Build();




// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

 
 
app.UseCors("AllowAll");


app.UseAuthentication(); // <-- MUST be before UseAuthorization
app.UseAuthorization();

app.MapControllers();



app.Run();
