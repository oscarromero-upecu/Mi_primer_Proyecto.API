


using Microsoft.AspNetCore.Identity;

namespace AccessoData
{
    // clase para ser la entidad con atributos para la base de datos
    public class Usuario : IdentityUser //hereda (IdentityUser) es una estructura definida con funciones de usuraio
    {
        //adicional a los parametros que da el IdentityUser se declara en este caso 03 atributos para la base de datos
        public string NombreUsuario { get; set; }
        public DateTime FechaDeRegistro { get; set; } 

        public ICollection<RegistroPedido> Pedidos { get; set; }
       
       
    }
}
