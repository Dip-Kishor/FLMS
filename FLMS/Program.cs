using FLMS.Data;
using FLMS.Services;
using FLMS.Services.PlayersRegistration;
using FLMS.Services.TokenValidation;
using FLMS.Services.User;
using FLMS.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<FLMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FLMSContext")));


builder.Services.AddScoped<SUser>();
builder.Services.AddScoped<SPlayerRegistration>();
builder.Services.AddScoped<SSeason>();
builder.Services.AddSingleton<ITokenBlacklistService, STokenBlacklistService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            //builder.WithOrigins("http://localhost:5173") 
            builder.WithOrigins("http://localhost:3000", "https://dipkishor.vercel.app", "http://localhost:5173")
                   .AllowCredentials()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["AccessToken:Issuer"],
            ValidAudience = builder.Configuration["AccessToken:Audience"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AccessToken:Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Extract token from HttpOnly cookie
                var token = context.HttpContext.Request.Cookies["accessToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdmin", policy => policy.RequireRole("SuperAdmin"));
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
    options.AddPolicy("All", policy => policy.RequireRole("SuperAdmin", "Admin", "User"));
    options.AddPolicy("SuperAdminAndAdmin", policy => policy.RequireRole("SuperAdmin", "Admin"));
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()  // This ensures that a user must be authenticated
        .Build();
});
// Add services to the container.
builder.Services.AddControllersWithViews();
// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(Options => {
    Options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    Options.OperationFilter<SecurityRequirementsOperationFilter>();
});
var app = builder.Build();
app.UseCors("AllowReactApp");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        options.RoutePrefix = string.Empty; // To serve Swagger at the app's root
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TokenMiddleWare>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
