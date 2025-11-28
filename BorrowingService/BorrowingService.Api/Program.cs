using AutoMapper;
using BorrowingService.Bll.Mapping;
using BorrowingService.Bll.Services;
using BorrowingService.Dal;
using BorrowingService.Dal.Interfaces;
using BorrowingService.Dal.Repositories;
using BorrowingService.Api.Middleware;
using Npgsql;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection");

// Logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Scoped NpgsqlConnection
builder.Services.AddScoped<NpgsqlConnection>(_ =>
{
    var conn = new NpgsqlConnection(connStr);
    conn.Open();
    return conn;
});

// Repositories
builder.Services.AddScoped<IReaderRepository, ReaderRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowingRepository, BorrowingRepository>();

// UnitOfWork
builder.Services.AddScoped<IUnitOfWork>(sp =>
{
    var conn = sp.GetRequiredService<NpgsqlConnection>();
    var books = sp.GetRequiredService<IBookRepository>();
    var borrowings = sp.GetRequiredService<IBorrowingRepository>();
    var readers = sp.GetRequiredService<IReaderRepository>();
    return new UnitOfWork(conn, books, borrowings, readers);
});

// BLL Services
builder.Services.AddScoped<ReaderService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<BorrowingAppService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(BorrowingProfile));

// HttpClient для CatalogService
builder.Services.AddHttpClient("catalog", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["CatalogService:BaseUrl"] ?? "http://localhost:5180");
});

// Controllers + JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
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
