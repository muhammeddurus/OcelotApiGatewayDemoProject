using CustomerAPI.Models;
using CustomerAPI.Services;
using CustomerAPI.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog.Events;
using Serilog;
using System.Text;
using System.Security.Claims;
using CustomerAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICustomerService, CustomerService>();


//var consulHost = "http://localhost:8500";

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File("Logs/logs.txt", rollingInterval: RollingInterval.Day))
                .CreateLogger();

builder.Logging.AddSerilog();

builder.Services.AddDbContext<ESaleDbContext>();

#region OpenIdDict
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.Authority = builder.Configuration["AuthenticationSettings:Authority"];
//        options.Audience = builder.Configuration["AuthenticationSettings:Audience"];
//        options.RequireHttpsMetadata = false;

//        options.Events = new()
//        {
//            OnTokenValidated = async context =>
//            {
//                if (context.Principal?.Identity is ClaimsIdentity claimsIdentity)
//                {
//                    Claim? scopeClaim = claimsIdentity.FindFirst("scope");
//                    if (scopeClaim is not null)
//                    {
//                        claimsIdentity.RemoveClaim(scopeClaim);
//                        claimsIdentity.AddClaims(scopeClaim.Value.Split(" ").Select(s => new Claim("scope", s)).ToList());
//                    }
//                }
//                await Task.CompletedTask;
//            }
//        };
//    }); 

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("APolicy", policy => policy.RequireClaim("scope","read"));
//    options.AddPolicy("BPolicy", policy => policy.RequireClaim("scope","write"));
//    options.AddPolicy("CPolicy", policy => policy.RequireClaim("scope","read","write"));
//    options.AddPolicy("DPolicy", policy => policy.RequireClaim("ozel-scope","ozel-claims-value"));
//});
#endregion
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisismysecretkeyforthisapigatewayproject")),
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

//builder.Services.AddConsulConfig(builder.Configuration);



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
//app.RegisterServiceToConsul(app.Lifetime);
app.Run();

