using AccessoData;
using AccessoData.Contexto;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using Negocio.Repositorio.IRepositorio;
using System.Data.Entity.Infrastructure;

namespace Pedidos_YA.API.Controllers
{
    //Controlador o delega a los objetos la tarea de desplegar la interfaz del usuario en este caso interfaz del repositorio
    //Plantillade controlador API
    [ApiController]
    //Ruta
    [Route("[Controller]")]
    public class PedidoController : ControllerBase //hereda propiedades y metodos para manejar solicitudes HTTP
    {
        private readonly IPedidoRepositorio _PedidoRepositorio;
        private readonly AppDbContext _db;

        //Contructor de IPedidoRepositorio que recibe un objeto (pedidoRepositorio) 
        public PedidoController(IPedidoRepositorio pedidoRepositorio, AppDbContext db)
        {
            _PedidoRepositorio = pedidoRepositorio;
            _db = db;
        }


        #region Pedidos
        //recibe e identifica una accion que admite el verbo de accion HTTPGET
        [Authorize]
        [HttpPost ("RegistrarPedido")]
        public async Task<IActionResult> RegistrarPedido(RegistroPedidoRequestDTO registroPedidoRequestDTO)
        {
            try
            {
                if (registroPedidoRequestDTO is null || !ModelState.IsValid)
                    return BadRequest(new MensajeResponseDTO { Mensaje = "Los campos estan incompletos" }); //retorna que es una solicitud mala
                return Ok(await _PedidoRepositorio.RegistrarPedido(registroPedidoRequestDTO));
            }
            catch (Exception)
            {

                return BadRequest(new MensajeResponseDTO { Mensaje = "Error de controlador" }); //retorna que es una solicitud mala
            }


        }

        [HttpGet("VerPedidos")]
        //Tarea asincronica en donde obtiene un booleano ok de la interfaz
        public async Task<IActionResult> ObtenerPedido()
        {
            //retorna un ok de respuesta http del objeto interfaz IRepositorio  
            return Ok(await _PedidoRepositorio.VerRegistroPedido());
        }

        [HttpGet("ObtenerPedidoporUsuario")]
        public async Task<IActionResult> ObtenerPedido(string idUsuario)
        {
            return Ok(await _PedidoRepositorio.VerRegistroPedido(idUsuario));
        }
       
        [Authorize]
        [HttpGet("ObtenerConsumoPedidos")]
        public async Task<IActionResult> ObtenerConsumoPedidos()
        {
            return Ok(await _PedidoRepositorio.VerConsumoPorUsuario());
        }

        [Authorize(Roles = Roles.Administrador)]
        [HttpDelete("EliminarPedido")]
        public async Task<IActionResult> EliminarPedido (int ID)
        {
            try
            {
                return Ok(await _PedidoRepositorio.EliminarPedido(ID));
            }
            catch (Exception)
            {

                return BadRequest(new MensajeResponseDTO { Mensaje = "Error de controlador" }); //retorna que es una solicitud mala
            }

        }
#endregion
    }
}
