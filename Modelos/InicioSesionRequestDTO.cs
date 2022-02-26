namespace Modelos
{

    //DTO objeto de transferencia de datos que sirve para tranportar datos entre porcesos
    public class InicioSesionRequestDTO
    {
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        
    }
}
