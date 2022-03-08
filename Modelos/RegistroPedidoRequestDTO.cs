namespace Modelos
{
    //DTO objeto de transferencia de datos que sirve para tranportar datos entre porcesos
    public class RegistroPedidoRequestDTO
    {
        public string UsuarioId { get; set; }
        public string NombreCliente { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioPedido { get; set; }
        public decimal Descuento { get; set; }
    }
}
