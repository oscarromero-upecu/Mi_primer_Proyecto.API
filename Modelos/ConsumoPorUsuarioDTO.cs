namespace Modelos
{
    public class ConsumoPorUsuarioDTO
    {
        public string UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public int CantidadPedidos { get; set; }
        public int Descuento { get; set; }
        public decimal TotalConsumido { get; set; }
    }
}
