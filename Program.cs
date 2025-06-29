using Microsoft.EntityFrameworkCore;
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
builder.Services.AddScoped<AuthService>(); 

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