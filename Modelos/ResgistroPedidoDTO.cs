namespace Modelos
{
    //DTO objeto de transferencia de datos que sirve para tranportar datos entre porcesos
    public class ResgistroPedidoDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public UsuarioDTO? Usuario { get; set; }
        public string NombrePedido { get; set; }
        public decimal PrecioPedido { get; set; }
        public DateTime FechaPedido { get; set; }
    }
}
