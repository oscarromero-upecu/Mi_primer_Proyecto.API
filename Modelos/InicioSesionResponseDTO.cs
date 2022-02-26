namespace Modelos
{
    //DTO objeto de transferencia de datos que sirve para tranportar datos entre porcesos
    public class InicioSesionResponseDTO
    {
        public bool AutenticaionExitosa { get; set; }
        public string MensajeError { get; set; }
        public string Token { get; set; }
        public UsuarioDTO Usuario { get; set; }
    }
}
