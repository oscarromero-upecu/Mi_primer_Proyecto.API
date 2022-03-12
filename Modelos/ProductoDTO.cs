using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    //DTO objeto de transferencia de datos que sirve para tranportar datos entre porcesos
    public class ProductoDTO
    {
        
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioProducto { get; set; }
        public DateTime FechaDeRegistro { get; set; }


    }
}
