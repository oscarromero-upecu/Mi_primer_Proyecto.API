﻿namespace Modelos
{
    //DTO objeto de transferencia de datos que sirve para tranportar datos entre porcesos
    public class ResgistroPedidoDTO
    {

        
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public string NombreCliente { get; set; }
        public decimal PrecioPedido { get; set; }
        public decimal Descuento { get; set; }
        public decimal TotalPedido { get; set; }
        public DateTime FechaDeRegistro { get; set; }


    }
}
