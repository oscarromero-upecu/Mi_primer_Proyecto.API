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

        //Icollection indica que ep producto puede tener atributos de la entidad Registro Pedido
        public ICollection<RegistroPedido> Pedidos { get; set; }



    }
}
