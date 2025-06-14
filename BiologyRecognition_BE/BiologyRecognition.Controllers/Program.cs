using BiologyRecognition.Application;
using BiologyRecognition.AutoMapper;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


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
});


builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddAutoMapper(typeof(AutoMapperAccount));

builder.Services.AddCors(options =>

{
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


var app = builder.Build();

  app.UseSwagger();
  app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowReactApp");
app.Run();
