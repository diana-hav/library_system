using AutoMapper;
using BorrowingService.Bll.Mapping;
using BorrowingService.Bll.Services;
using BorrowingService.Dal.Interfaces;
using BorrowingService.Dal.Repositories;
using BorrowingService.Api.Middleware;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Connection string
var connStr = builder.Configuration.GetConnectionString("DefaultConnection");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddScoped<IReaderRepository>(_ => new ReaderRepository(connStr!));
builder.Services.AddScoped<ReaderService>();
builder.Services.AddScoped<BookService>(sp =>
    new BookService(connStr!, sp.GetRequiredService<IMapper>()));
builder.Services.AddAutoMapper(typeof(BorrowingProfile));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Borrowing Service API",
        Version = "v1",
        Description = "API ��� ������ �� ���������� ���� � ��������",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Library System Team",
            Email = "support@library.local"
        }
    });
});
builder.Services.AddScoped<BorrowingAppService>(sp =>
    new BorrowingAppService(connStr!, sp.GetRequiredService<IMapper>()));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:5174")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();
app.Run();
