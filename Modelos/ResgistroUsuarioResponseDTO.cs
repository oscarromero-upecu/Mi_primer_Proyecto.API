namespace Modelos
{
    //DTO objeto de transferencia de datos que sirve para tranportar datos entre porcesos
    public class ResgistroUsuarioResponseDTO
    {
        public bool ResgistroSatisfactorio { get; set; }
        public IEnumerable<string> Errores { get; set; }
    }
}
