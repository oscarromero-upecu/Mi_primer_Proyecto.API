namespace Modelos
{
    //DTO objeto de transferencia de datos que sirve para tranportar datos entre porcesos
    public class UsuarioDTO
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public decimal DescuentoPedido { get; set;}
        public DateTime FechaPedido { get; set; }

    }
}