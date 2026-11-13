using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoMoveisMedeiros.Api.Data;
using ProjetoMoveisMedeiros.Api.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();


// Conexão com o banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


// Serviço de autenticação
builder.Services.AddScoped<AuthenticationService>();


// Serviço para gerar e verificar o hash das senhas
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();


// Configuração JWT
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Chave JWT não configurada.");


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;


    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
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
            Encoding.UTF8.GetBytes(jwtKey)
        ),


        ClockSkew = TimeSpan.FromSeconds(30)
    };
});


// Permissão de acesso às rotas protegidas
builder.Services.AddAuthorization();


var app = builder.Build();


app.UseHttpsRedirection();


// Verifica o token enviado pelo usuário
app.UseAuthentication();


// Controla o acesso às rotas
app.UseAuthorization();


app.MapControllers();


app.Run();