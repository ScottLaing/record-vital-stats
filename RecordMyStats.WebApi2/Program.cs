using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using RecordMyStats.WebApi2.Services;
using RecordMyStats.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{

    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Config.Secrets.JwtTokenPhrase))
    };

});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();

    options.OperationFilter<AuthorizeOperationFilter>();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "description",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    var info = new OpenApiInfo()
    {
        Version = "1",
        Title = "RecordMyStats.WebApi2",
        Description = "Daily Vitalz - stats API 1.0 6-28-2024",
        Contact = new OpenApiContact() { Name = "Scott Laing", Email = "scottlaing@gmail.com",
            Url = new Uri("https://scottclaing.net")},
        License = new OpenApiLicense() { Name = "Scott"}
    };

    options.SwaggerDoc("v1", info);
});
//builder.Services.AddAuthentication().Add

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

//app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
