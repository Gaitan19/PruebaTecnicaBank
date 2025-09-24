using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBank.Core.Interfaces;
using PruebaTecnicaBank.Core.Services;
using PruebaTecnicaBank.Infrastructure.Mappings;
using PruebaTecnicaBank.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using PruebaTecnicaBank.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);


// DbContext con SQLite
builder.Services.AddDbContext<BankDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<ICuentaRepositorio, CuentaRepositorio>();
builder.Services.AddScoped<ITransaccionRepositorio, TransaccionRepositorio>();

// Servicios
builder.Services.AddScoped<IClienteServicio, ClienteServicio>();
builder.Services.AddScoped<ICuentaServicio, CuentaServicio>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BankDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

