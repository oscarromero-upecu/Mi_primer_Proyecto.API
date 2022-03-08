using AccessoData.Contexto;
using Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AccessoData.InicializarDB
{
    //clase que va a inicializar la base de datos, agregara datos en acso de que necesite
    public class InicializadorDB : IInicializadorDB
    {

        private readonly UserManager<Usuario> _userManager; //para crear nuestros usuarios
        private readonly RoleManager<IdentityRole> _roleManager; //para los roles
        private readonly AppDbContext _db; // para migraciones

        public InicializadorDB(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        
        public void InicalizarDB()
        {
            //consultar si es que tenemos migraciones pendientes
            if (_db.Database.GetPendingMigrations().Count() > 0)
                //true ejecuta la migracion
                _db.Database.Migrate();

            //consultar si es que existe el rol administrador
            if (!_roleManager.RoleExistsAsync(Roles.Administrador).GetAwaiter().GetResult())//getawaiter crea de async a asincrono ya que ya esta inicializando la base de datos
                //si esque no existe el rol que me cree un rol administrador sincrono con el getawaiter
                _roleManager.CreateAsync(new IdentityRole(Roles.Administrador)).GetAwaiter().GetResult();
            else
                return;

            //crear usuario administrador
            var usuario = new Usuario
            {
                NombreUsuario = "Admin",
                UserName = "chicuritas@example.com",
                Email = "chicuritas@example.com",
                EmailConfirmed = true
            };

            //una vez que ya esta la variable usuario agregamos el asuario
            _userManager.CreateAsync(usuario, "Admin*1234").GetAwaiter().GetResult();

            //agragamos el rol al ausuario adminstrador
            _userManager.AddToRoleAsync(usuario, Roles.Administrador).GetAwaiter().GetResult();
        }
    }
}
