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
        public string UserId { get; set; }
        [ForeignKey ("UserId")]
        public Usuario Usuario { get; set; }
        public string NombrePedido { get; set; }
        public decimal PrecioPedido { get; set; }
        public DateTime FechaPedido { get; set; } 

    }
}
