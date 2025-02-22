using Microsoft.EntityFrameworkCore;
using WebApiAfternoon.Data;
using WebApiAfternoon.Formatters;
using WebApiAfternoon.Middlewares;
using WebApiAfternoon.Repositories.Abstract;
using WebApiAfternoon.Repositories.Concrete;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    //   options.OutputFormatters.Insert(0, new VCardOutputFormatter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("http://localhost:5175") // Change your React URL
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<StudentDbContext>(opt =>
{
    opt.UseSqlServer(conn);
});

var app = builder.Build();

app.UseCors("AllowReactApp");

app.UseMiddleware<GlobalErrorHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  

app.UseCustomAuthMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();
