using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MyApiTest.Data;
using MyApiTest.Services;
using MyApiTest.Interfaces;
using MyApiTest.Repositories;
using DotNetEnv;

Env.Load(); 

var builder = WebApplication.CreateBuilder(args);

// ✅ REGISTER Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(Env.GetString("POSTGRES_CONNECTION")));

// ✅ REGISTER Repositories 
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ✅ REGISTER Services  
builder.Services.AddControllers();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthService>();

// ✅ JWT Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Configure pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

// ✅ Test Database Connection
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Console.WriteLine("DB Connected: " + db.Database.CanConnect());
    
    // ✅ Test Repository 
    var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    Console.WriteLine("UserRepository registered successfully!");
}

app.Run();