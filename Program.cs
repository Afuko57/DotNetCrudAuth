using Microsoft.EntityFrameworkCore;
using MyApiTest.Data;
using MyApiTest.Services;
using DotNetEnv;

Env.Load(); 

var builder = WebApplication.CreateBuilder(args);

// REGISTER Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(Env.GetString("POSTGRES_CONNECTION")));

// REGISTER Services
builder.Services.AddControllers();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<AuthService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Console.WriteLine("DB Connected: " + db.Database.CanConnect());
}

app.Run();
