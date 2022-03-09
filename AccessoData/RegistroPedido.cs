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

        #region Claves Foraneas
        public string UsuarioId { get; set; }
        //declaramos una variable este caso "UsuarioId" y luego en la opcion ForeignKey
        //la renombramos para que sea nuestra clave foranea de la entidad Usuario
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }


        public int ProductoId { get; set; }
        //declaramos una variable este caso "ProductoId" y luego en la opcion ForeignKey
        //la renombramos para que sea nuestra clave foranea de la entidad Producto
        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }
        #endregion

        public string NombreProducto { get; set; }
        public string NombreCliente { get; set; }
        public decimal PrecioPedido { get; set; }
        public decimal Descuento { get; set; }
        public decimal TotalPedido { get; set; }
        public DateTime FechaDeRegistro { get; set; }

    }

}

