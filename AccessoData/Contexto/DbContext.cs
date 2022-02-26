using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccessoData.Contexto
{
    //Conexion a SQLServer para crear entidades
    public class AppDbContext : IdentityDbContext //es el contexto ya extructurado de una conexion a SQL
    {
        public AppDbContext(DbContextOptions<AppDbContext> opciones) : base(opciones)
        {

        }

        //Crea la entidad Usuario en la bse de datos tomando la clase Usuario
        public DbSet<Usuario> Usuarios { get; set; }

        //Crea la entidad RegistroPedido en la bse de datos tomando la clase RegistroPedido
        public DbSet<RegistroPedido> RegistroPedido { get; set; }
    }
}
