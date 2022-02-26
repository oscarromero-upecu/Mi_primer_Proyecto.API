using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using Negocio.Repositorio.IRepositorio;

namespace Pedidos_YA.API.Controllers
{
    //Controlador o delega a los objetos la tarea de desplegar la interfaz del usuario en este caso interfaz del repositorio
    //Plantillade controlador API
    [ApiController]
    //Ruta
    [Route("[controller]")]
    public class PedidoController : ControllerBase //hereda proiedades y metodos para manejar solicitudes HTTP
    {
        private readonly IPedidoRepositorio _PedidoRepositorio;

        //Contructor de IPedidoRepositorio que recibe un objeto (pedidoRepositorio) 
        public PedidoController(IPedidoRepositorio pedidoRepositorio)
        {
            _PedidoRepositorio = pedidoRepositorio;
        }

        //recibe e identifica una accion que admite el verbo de accion HTTPGET
        [HttpGet]
        //Tarea asincronica en donde obtiene un booleano ok de la interfaz
        public async Task<IActionResult> ObtenerPedido()
        {
            //retorna un ok de respuesta http del objeto interfaz IRepositorio  
            return Ok(await _PedidoRepositorio.VerRegistroPedido());
        }

        
        [HttpPost]
        public async Task<IActionResult> Crear(ResgistroPedidoDTO registroPedidoDTO)
        {
            registroPedidoDTO.FechaPedido = DateTime.Now;
            var resultado = await _PedidoRepositorio.RegistrarPedido(registroPedidoDTO);
            return Ok(resultado);
        }

       
        [HttpGet("{idUsuario}")]
        public async Task<IActionResult> ObtenerPedido(string idUsuario)
        {
            return Ok(await _PedidoRepositorio.VerRegistroPedido(idUsuario));
        }

        ////
        //[Authorize (Roles = Roles.Administrador)]
        //[HttpGet("[action]")]
        //public async Task<IActionResult> ObtenerConsumoPedidos( )
        //{
        //    return Ok(await _PedidoRepositorio.VerConsumoPorUsuario());
        //}


    }
}
