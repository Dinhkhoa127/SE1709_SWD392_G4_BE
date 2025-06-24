using BiologyRecognigition.AutoMapper;
using BiologyRecognition.Application;
using BiologyRecognition.AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Set default port for Kestrel server
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(7032, listenOptions =>
//    {
//        listenOptions.UseHttps();  // Sử dụng HTTPS
//    });
//});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "🌿 Biology Recognition API",
        Version = "v1.0",
        Description = "An API for identifying plants and retrieving textbook content.",
    });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "JWT Authorization header using the access token",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };
    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, new string[] { } }
    });
});

// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JwtConfig");
var key = jwtSettings["Key"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Cho API dùng JWT
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme) // Đăng nhập Google cần Cookie scheme để tạm giữ ClaimsPrincipal
    .AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Nếu có Authorization Header (Bearer) thì dùng nó
            var bearerToken = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(bearerToken))
            {
                return Task.CompletedTask; // Đã có token từ header
            }

            // Nếu không có token trong header, kiểm tra cookie
            var token = context.Request.Cookies["access_token"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse(); // Ngăn không cho ASP.NET Core tự trả về mặc định
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                status = 401,
                message = "Unauthorized: Token is missing or invalid"
            });
            return context.Response.WriteAsync(result);
        }
    };
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"];
    options.ClientSecret = builder.Configuration["Google:ClientSecret"];
    options.CallbackPath = "/google/callback";
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddAutoMapper(typeof(AutoMapperAccount));
builder.Services.AddAutoMapper(typeof(AutoMappperSubject));




builder.Services.AddCors(options =>

{
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyMethod()
                          .AllowCredentials() // Cho phép cookie
                          .AllowAnyHeader());
});


var app = builder.Build();

  app.UseSwagger();
  app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseCors("AllowReactApp"); // Phải use CORS trước Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
