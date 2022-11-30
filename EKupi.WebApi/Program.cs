using EKupi.Application;
using EKupi.Application.Customers.Commands;
using EKupi.Application.Services;
using EKupi.Domain.Entities;
using EKupi.Infrastructure;
using EKupi.Infrastructure.AppSettings;
using EKupi.Infrastructure.Interfaces;
using EKupi.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Reflection;
using System.Security.Principal;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"))
    .LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddIdentity<Customer, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

builder.Services.AddSingleton<ITokenSettings>(options => {
    return builder.Configuration.GetSection(nameof(TokenSettings)).Get<TokenSettings>();
});
var tokenSettings = builder.Configuration.GetSection(nameof(TokenSettings)).Get<TokenSettings>();

builder.Services.AddMediatR(typeof(LoginCustomerCommand).GetTypeInfo().Assembly);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options => {
        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var auth = context.Request.Cookies["authCookie"];

                if (auth is not null)
                {
                    context.Request.Headers["Authorization"] = "Bearer " + auth;
                    //context.Token = auth;
                }
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    }
);

builder.Services.AddScoped<IPrincipal>(f => f.GetRequiredService<IHttpContextAccessor>().HttpContext?.User);

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandler>();

app.MapControllers();

app.Run();
