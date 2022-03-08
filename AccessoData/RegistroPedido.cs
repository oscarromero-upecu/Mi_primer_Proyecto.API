using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessoData
{
    // clase para ser la entidad con atributos para la base de datos
    public class RegistroPedido
    {
        //llave Id 
        [Key]
        public int Id { get; set; }
        //clave foranea
        
        public string UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }
        public string NombreProducto { get; set; }

        public string NombreCliente { get; set; }
        public decimal PrecioPedido { get; set; }
        public decimal Descuento { get; set; }
        public decimal TotalPedido { get; set; }
        public DateTime FechaDeRegistro { get; set; }

    }

}

