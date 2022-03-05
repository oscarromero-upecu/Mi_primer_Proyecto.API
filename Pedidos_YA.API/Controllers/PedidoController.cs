using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using Negocio.Repositorio.IRepositorio;
using System.Security.Claims;

namespace Pedidos_YA.API.Controllers
{
    //Controlador o delega a los objetos la tarea de desplegar la interfaz del usuario en este caso interfaz del repositorio
    //Plantillade controlador API
    [ApiController]
    //Ruta
    [Route("[PedidoController]")]
    public class PedidoController : ControllerBase //hereda propiedades y metodos para manejar solicitudes HTTP
    {
        private readonly IPedidoRepositorio _PedidoRepositorio;

        //Contructor de IPedidoRepositorio que recibe un objeto (pedidoRepositorio) 
        public PedidoController(IPedidoRepositorio pedidoRepositorio)
        {
            _PedidoRepositorio = pedidoRepositorio;
        }

        [Authorize(Roles = Roles.Administrador)]
        //recibe e identifica una accion que admite el verbo de accion HTTPGET
        [HttpGet("VerPedidos")]
        //Tarea asincronica en donde obtiene un booleano ok de la interfaz
        public async Task<IActionResult> ObtenerPedido()
        {
            //retorna un ok de respuesta http del objeto interfaz IRepositorio  
            return Ok(await _PedidoRepositorio.VerRegistroPedido());
        }

        [Authorize]
        [HttpPost ("RegistrarPedido")]
        public async Task<IActionResult> RegistrarPedido(ResgistroPedidoDTO registroPedidoDTO)
        {
            //mapeo 
            var idUsuario = (HttpContext.User.Identity as ClaimsIdentity)?
                .FindFirst("Id")?.Value; //busco el id

            //validacion si esque el id del usuario no es administrador
            if (idUsuario != registroPedidoDTO.UserId && !User.IsInRole(Roles.Administrador))
                return Unauthorized("No tiene permisos para realizar esta operacion");

            registroPedidoDTO.FechaPedido = DateTime.Now;// agregar la fecha actual
            var resultado = await _PedidoRepositorio.RegistrarPedido(registroPedidoDTO);// variable asignada con la instacion del registroPedido
            return Ok(resultado);
        }

       
        [HttpGet("ObtenerPedidoporUsuario")]
        public async Task<IActionResult> ObtenerPedido(string idUsuario)
        {
            return Ok(await _PedidoRepositorio.VerRegistroPedido(idUsuario));
        }

        //
        [Authorize(Roles = Roles.Administrador)]
        [HttpGet("ObtenerConsumoPedidos")]
        public async Task<IActionResult> ObtenerConsumoPedidos()
        {
            return Ok(await _PedidoRepositorio.VerConsumoPorUsuario());
        }


    }
}
