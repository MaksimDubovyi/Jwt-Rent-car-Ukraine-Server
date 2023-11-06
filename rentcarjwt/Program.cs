using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using rentcarjwt.MessageHub;
using rentcarjwt.Model;
using rentcarjwt.Model.Data;
using servercar.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Для використання функціональності бібліотеки SignalR,
// у додатку необхідно зареєструвати відповідні послуги
builder.Services.AddSignalR();

// Add services to the container.

MySqlConnection connection = new(builder.Configuration.GetConnectionString("ukraine"));//ukraine- назва рядка підключення в файлі azuresettings.json 

builder.Services.AddDbContext<DataContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection),
        serverOptions => serverOptions
        .MigrationsHistoryTable(  tableName: HistoryRepository.DefaultTableName,schema: DataContext.SchemaName)
        .SchemaBehavior( MySqlSchemaBehavior.Translate,(schema, table) => $"{schema}_{table}")));

builder.Services.AddCors(); // добавляем сервисы CORS
builder.Services.AddMyService();

builder.Configuration.AddJsonFile("appsettings.json", false);//Підключаемо файл appsettings.json
//таємні ключі які знає лише сервер
//---------------------------------------start JWT token--------------------------------------
var secretKey = builder.Configuration.GetSection("Jwt:Secret").Value;
var issuer = builder.Configuration.GetSection("Jwt:Issuer").Value;
var audience = builder.Configuration.GetSection("Jwt:Audience").Value;
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));


builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

 .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = false,
          ValidIssuer = issuer,
          ValidateAudience = false,
          ValidAudience=audience,
          ValidateLifetime = true,
          IssuerSigningKey=signingKey,
          ValidateIssuerSigningKey = true,
          ClockSkew=TimeSpan.Zero
        
      
      };
  });

//---------------------------------------end JWT token--------------------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();




var app = builder.Build();

app.UseCors(builder => builder
 //.WithOrigins("https://rcua.azurewebsites.net", "https://rentcarua.azurewebsites.net", "http://localhost:8081", "http://localhost:4200") //даємо дозвіл звиртатись до сервера з адреси  https://musicsua.azurewebsites.net
//.AllowAnyOrigin()
.SetIsOriginAllowed(host=>true)    
.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();
//app.UseCors("cors");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MessagesHub>("/MessagesHub");   // ChatHub буде обробляти запити на шляху /chat


app.Run();


