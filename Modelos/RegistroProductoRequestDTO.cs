namespace Modelos
{
    //DTO objeto de transferencia de datos que sirve para tranportar datos entre porcesos
    public class RegistroProductoRequestDTO
    {
        public string UsuarioId { get; set; }

        public string NombreProducto { get; set; }
        public decimal PrecioProducto { get; set; }
    }
}
