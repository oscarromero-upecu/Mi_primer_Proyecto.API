using AccessoData;
using AccessoData.Contexto;
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


        [Authorize] /*(Roles = Roles.Administrador)]*/
        //recibe e identifica una accion que admite el verbo de accion HTTPGET

        [Authorize]
        [HttpPost ("RegistrarPedido")]
        public async Task<IActionResult> RegistrarPedido([FromBody] RegistroPedidoRequestDTO registroPedidoRequestDTO)
        {
            if (registroPedidoRequestDTO is null || !ModelState.IsValid)
                return BadRequest(); //retorna que es una solicitud mala
            var productoid = _db.Producto.Where(p => p.NombreProducto == registroPedidoRequestDTO.NombreProducto)
                .Select(p => p.Id).FirstOrDefault();
            //nuevo Pedido
            var Pedido = new RegistroPedido
            {
                ProductoId = productoid,
                UsuarioId = registroPedidoRequestDTO.UsuarioId,
                NombreCliente = registroPedidoRequestDTO.NombreCliente,
                NombreProducto = registroPedidoRequestDTO.NombreProducto,
                PrecioPedido = registroPedidoRequestDTO.PrecioPedido,
                Descuento = registroPedidoRequestDTO.Descuento,
                TotalPedido = (registroPedidoRequestDTO.PrecioPedido-registroPedidoRequestDTO.Descuento),
                FechaDeRegistro = DateTime.Now,
            };

            if (Pedido== null) //valida si el pedido esta registrado (logrado)
                return BadRequest(new ResgistroPedidoResponseDTO //si no es stisfactorio ponemos un mala solictud 
                {
                    ResgistroSatisfactorio = false,
                    Mensaje="Error al registrar",
                });

            //luego como "nuevoPedido" agrega a la base de datos de "RegistroPedido" el registroPedidosDTO "PedidoDTO"
            var nuevoPedido = _db.RegistroPedido.Add(Pedido);

            //    //await(esperar) es el break para la tarea y guarda los cambios asincronicos en la base de datos
            await _db.SaveChangesAsync();


            return Ok(new ResgistroPedidoResponseDTO { ResgistroSatisfactorio = true, TotalPedido = Pedido.TotalPedido, });
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

        //
        //[Authorize(Roles = Roles.Administrador)]
        [HttpGet("ObtenerConsumoPedidos")]
        public async Task<IActionResult> ObtenerConsumoPedidos()
        {
            return Ok(await _PedidoRepositorio.VerConsumoPorUsuario());
        }


    }
}
