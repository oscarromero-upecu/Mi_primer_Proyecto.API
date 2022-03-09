namespace Modelos
{
    //DTO objeto de transferencia de datos que sirve para tranportar datos entre porcesos
    public class MensajeResponseDTO
    {
        public bool ResgistroSatisfactorio { get; set; }
        public decimal TotalPedido { get; set; }
        public string Mensaje { get; set; }
    }
}
