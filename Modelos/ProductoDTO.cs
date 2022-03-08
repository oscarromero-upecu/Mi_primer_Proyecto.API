using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class ProductoDTO
    {
        
        public string UsuarioId { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioProducto { get; set; }
        public DateTime FechaDeRegistro { get; set; }


    }
}
