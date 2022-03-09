namespace Modelos
{
    //DTO objeto de transferencia de datos que sirve para tranportar datos entre porcesos
    public class ConsumoPorUsuarioDTO
    {
        public string UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCliente { get; set; }
        public int CantidadPedidos { get; set; }
        public decimal TotalConsumido { get; set; }
    }
}
