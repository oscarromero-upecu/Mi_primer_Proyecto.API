using AccessoData;
using AccessoData.Contexto;
using AccessoData.InicializarDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Negocio.Repositorio;
using Negocio.Repositorio.IRepositorio;
using Pedidos_YA.API;
using System.Text;

//Biulder es un patron de diseno que permite construir objetos complejos paso a paso y no necesita que tengan ana interfaz comun

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Biulder (patron) de diseno estandar para la confirmacion de la primera frase (Bearer) de autienticacion que iria junto al token 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pedidos YA! API", Version = "v1" });
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

//Construcctor definido para la conexion a la base de datos SQLServer al DBvirtual
builder.Services.AddDbContext<AppDbContext>(opciones => opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnetcion")));

// arranque con los parametros Identity
builder.Services.AddIdentity<Usuario, IdentityRole>().AddDefaultTokenProviders() 
    .AddEntityFrameworkStores<AppDbContext>();// apunta a la base de datos 

//arranque con los parametros Configuracion Jwt (token)
var sectionConfiguracionJwt = builder.Configuration.GetSection("ConfiguracionJwt"); //el Configuration.GetSection va a appsenting y busca la seccion "ConfiguracionJwt"
builder.Services.Configure<ConfiguracionJWT>(sectionConfiguracionJwt); //apunta a la seccion JTW de appsenting

//arranque con los parametros Autenticacion
var configuracionJwt = sectionConfiguracionJwt.Get<ConfiguracionJWT>();
var secreto = Encoding.ASCII.GetBytes(configuracionJwt.Secreto);


//Biulder (patron) de diseno estandar para la autenticacion de la primera frase (Bearer) con el token 
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secreto),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Repositorios: AddScoped realiza la instancia del tipo de servicio, es decir llamas a una copia exacta de la clase
builder.Services.AddScoped<IPedidoRepositorio, PedidosRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();

//Inicializador DB, agregamos la interfaz 
builder.Services.AddScoped<IInicializadorDB, InicializadorDB>();

var app = builder.Build();
//Swagger
app.UseSwagger();
app.UseSwaggerUI();

//Inicializa con nuevo metodo
InicializarDB();

// Configure the HTTP request pipeline.

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

//metodo que sirve para utilizar el objeto creando un tipo scope
//con esto al momento que ejecute el programa ya me va a dar el inicializador
void InicializarDB()
{
    using (var scorpe = app.Services.CreateScope())
    {
        var inicializador = scorpe.ServiceProvider.GetRequiredService<IInicializadorDB>();
        inicializador.InicalizarDB();
    }
}