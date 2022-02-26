using AccessoData.Contexto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Negocio.Repositorio;
using Negocio.Repositorio.IRepositorio;

//Biulder es un patron de diseno que permite construir objetos complejos paso a paso y no necesita que tengan ana interfaz comun

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Taller API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Bearer and then token in the field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                });
}); 


//Construcctor definido para la conexion a lla base de datos
builder.Services.AddDbContext<AppDbContext>(opciones => opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnetcion")));

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Repositorios
builder.Services.AddScoped<IPedidoRepositorio, PedidosRepositorio>();
//builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

////Inicializador DB
//builder.Services.AddScoped<IInicializadorDB, InicializadorDB>();

var app = builder.Build();
//Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
