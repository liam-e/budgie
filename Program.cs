using Microsoft.EntityFrameworkCore;
using BudgetApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

// builder.Services.AddAuthentication(x =>
// {
//     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
// }).AddJwtBearer(x =>
// {
//     x.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidIssuer = config["JwtSettings:Issuer"],
//         ValidAudience = config["JwtSettings:Audience"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//     };
// });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("JwtSettings", options))
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("CookieSettings", options));

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddDbContext<BudgetContext>(opt => opt.UseInMemoryDatabase("BudgetApi"));
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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
