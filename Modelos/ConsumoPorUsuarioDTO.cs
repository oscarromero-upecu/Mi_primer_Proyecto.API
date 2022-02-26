namespace Modelos
{
    public class ConsumoPorUsuarioDTO
    {
        public string IdUsuario { get; set; }
        public string Nombre { get; set; }
        public int CantidadPedidos { get; set; }
        public decimal TotalAPagar { get; set; }
    }
}
