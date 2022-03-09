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
        public async Task<IActionResult> RegistrarPedido([FromBody] RegistroPedidoRequestDTO registroPedidoRequestDTO)
        {
            if (registroPedidoRequestDTO.UsuarioId == "string" ||
                registroPedidoRequestDTO.NombreCliente == "string" ||
                registroPedidoRequestDTO.NombreProducto == "string"||
                registroPedidoRequestDTO.PrecioPedido == 0)
                return BadRequest(new MensajeResponseDTO { Mensaje = "Los campos estan incompletos" }); //retorna que es una solicitud mala
            try
            {
                //Consulta a la base de datos para obtener el id de producto
                var productoid = _db.Producto.Where(p => p.NombreProducto == registroPedidoRequestDTO.NombreProducto)
                    .Select(p => p.Id).FirstOrDefault(); //firstOrdefault obtine el registro en la consulta

                //Si la consulta no es satisfactoria
                if (productoid <= 0)
                {
                    return BadRequest(new MensajeResponseDTO 
                    { Mensaje = "El Nombre del Producto no existe (Cree un nuevo) o ingrese nombre del producto correcto" 
                    }); //retorna que es una solicitud mala
                }

                //nuevo Pedido
                var Pedido = new RegistroPedido
                {
                    ProductoId = productoid,
                    UsuarioId = registroPedidoRequestDTO.UsuarioId,
                    NombreCliente = registroPedidoRequestDTO.NombreCliente,
                    NombreProducto = registroPedidoRequestDTO.NombreProducto,
                    PrecioPedido = registroPedidoRequestDTO.PrecioPedido,
                    Descuento = registroPedidoRequestDTO.Descuento,
                    TotalPedido = (registroPedidoRequestDTO.PrecioPedido - registroPedidoRequestDTO.Descuento),
                    FechaDeRegistro = DateTime.Now,
                };

                if (Pedido == null) //valida si el pedido esta registrado (logrado)
                    return BadRequest(new MensajeResponseDTO //si no es stisfactorio ponemos un mala solictud 
                    {
                        ResgistroSatisfactorio = false,
                        Mensaje = "Error al registrar",
                    });

                //luego como "nuevoPedido" agrega a la base de datos de "RegistroPedido" el registroPedidosDTO "PedidoDTO"
                var nuevoPedido = _db.RegistroPedido.Add(Pedido);

                //    //await(esperar) es el break para la tarea y guarda los cambios asincronicos en la base de datos
                await _db.SaveChangesAsync();


                return Ok(new MensajeResponseDTO { ResgistroSatisfactorio = true, TotalPedido = Pedido.TotalPedido, Mensaje = "GRACIAS!"});
            }
            catch (Exception)
            {

                return BadRequest(new MensajeResponseDTO //si no es stisfactorio ponemos un mala solictud 
                {
                    ResgistroSatisfactorio = false,
                    Mensaje = "Error al registrar",
                });

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
                var registro = _db.RegistroPedido.Where(r=> r.Id == ID).FirstOrDefault(); // Consulta el id
                if (registro == null)
                {
                    return BadRequest("Error al Elminar");
                }
                _db.RegistroPedido.Remove(registro);//elimina el registro
                await _db.SaveChangesAsync(); //guarda los cambios
                return RedirectToAction("Index");//redirecciona el index
            }
            catch (DbUpdateException /*ex*/)
            {

                return BadRequest("Error al Elminar");
            }
          
        }
#endregion
    }
}
