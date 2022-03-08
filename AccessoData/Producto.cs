using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessoData

{
    public class Producto
    {
        ////llave Id 
        [Key]
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioProducto { get; set; }
        public DateTime FechaDeRegistro { get; set; }
        public ICollection<RegistroPedido> Pedidos { get; set; }



    }
}
