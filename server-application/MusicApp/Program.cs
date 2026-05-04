using CatalogApp.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});
builder.Services.AddControllers();



// Настроим CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin() // Разрешаем любой источник
               .AllowAnyMethod() // Разрешаем любые методы
               .AllowAnyHeader(); // Разрешаем любые заголовки
    });
});

// Добавляем сервисы для работы с API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Используем CORS
app.UseCors("AllowAllOrigins"); 

app.UseStaticFiles();  

// Настроим Swagger для документации
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

// Регистрируем контроллеры
app.MapControllers();

app.Run();