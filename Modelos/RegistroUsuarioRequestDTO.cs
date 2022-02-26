﻿namespace Modelos
{
    //DTO objeto de transferencia de datos que sirve para tranportar datos entre porcesos
    public class RegistroUsuarioRequestDTO
    {
        public string Nombre { get; set; }
        public string Email { get; set; }

       
        public string NombrePedido { get; set; }
        public decimal DescuentoPedido { get; set; }
        public string Contrasena { get; set; }
        public bool RegistroSatisfactorio { get; set; }
    }
}
