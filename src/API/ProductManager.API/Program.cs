using Microsoft.EntityFrameworkCore;
using ProductManager.API.Middleware;
using ProductManager.Application;
using ProductManager.Infrastructure;
using ProductManager.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog para logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Iniciando a aplicação...");

    // Configuração dos serviços
    builder.Services.AddControllers();
    
    // Configuração do Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Configuração da aplicação
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // Configuração do CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });

    var app = builder.Build();

    // Configuração do pipeline de requisições HTTP
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionMiddleware>();
    
    app.UseSerilogRequestLogging();
    
    app.UseHttpsRedirection();
    
    app.UseCors("AllowAll");
    
    app.UseAuthorization();

    app.MapControllers();

    // Aplicar migrações do banco de dados apenas se o banco de dados não existir
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            
            // Verificar se o banco de dados já existe
            if (await context.Database.CanConnectAsync())
            {
                Log.Information("Banco de dados já existe. Pulando migrações.");
            }
            else
            {
                Log.Information("Banco de dados não encontrado. Aplicando migrações...");
                try
                {
                    await context.Database.MigrateAsync();
                    Log.Information("Migrações do banco de dados aplicadas com sucesso.");
                }
                catch (Exception migrateEx) when (migrateEx.Message.Contains("There is already an object named"))
                {
                    // Ignorar erros de tabelas já existentes
                    Log.Warning(migrateEx, "Algumas tabelas já existem no banco de dados. Continuando...");
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ocorreu um erro ao verificar/aplicar as migrações do banco de dados.");
        }
    }

    Log.Information("Aplicação iniciada com sucesso.");
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação encerrou inesperadamente.");
}
finally
{
    Log.CloseAndFlush();
}
