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
    [Route("[Controller]/[action]")]
    public class ProductoController : ControllerBase //hereda propiedades y metodos para manejar solicitudes HTTP
    {

        private readonly IProductoRepositorio _productoRepositorio;
        private readonly AppDbContext _db;

        public ProductoController(IProductoRepositorio productoRepositorio, AppDbContext db)
        {
            _productoRepositorio = productoRepositorio;
            _db = db;
        }

        #region Producto
        [Authorize(Roles = Roles.Administrador)]
        [HttpPost]
        public async Task<IActionResult> RegistrarProducto([FromBody] RegistroProductoRequestDTO registroProductoRequestDTO)
        {
            try
            {
                if (registroProductoRequestDTO.UsuarioId == "string" ||
                 registroProductoRequestDTO.NombreProducto == "string" ||
                 registroProductoRequestDTO.PrecioProducto == 0)
                    return BadRequest(new MensajeResponseDTO { Mensaje = "Los campos estan incompletos" }); //retorna que es una solicitud mala
                                                                                                             //nuevo usuario
                var Producto = new Producto
                {
                    UsuarioId = registroProductoRequestDTO.UsuarioId,
                    NombreProducto = registroProductoRequestDTO.NombreProducto,
                    PrecioProducto = registroProductoRequestDTO.PrecioProducto,
                    FechaDeRegistro = DateTime.Now,
                };
                //luego como "nuevoPedido" agrega a la base de datos de "RegistroPedido" el registroPedidosDTO "PedidoDTO"
                var nuevoPedido = _db.Producto.Add(Producto);

                //    //await(esperar) es el break para la tarea y guarda los cambios asincronicos en la base de datos
                await _db.SaveChangesAsync();

                if (Producto == null) //valida si el pedido esta registrado (logrado)
                    return BadRequest(new MensajeResponseDTO //si no es stisfactorio ponemos un mala solictud 
                    {
                        ResgistroSatisfactorio = false,
                        Mensaje = "Error al registrar"
                    });

                return Ok(new MensajeResponseDTO { ResgistroSatisfactorio = true, Mensaje = "GRACIAS!" });
            }
            catch (Exception)
            {
                return BadRequest(new MensajeResponseDTO //si no es stisfactorio ponemos un mala solictud 
                {
                    ResgistroSatisfactorio = false,
                    Mensaje = "Error al registrar"
                });
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> ObtenerProducto()// 
        {

            return Ok(await _productoRepositorio.ObtenerProducto());

        }

        [Authorize(Roles = Roles.Administrador)]
        [HttpDelete("EliminarProducto")]
        public async Task<IActionResult> EliminarProducto(int ID)
        {
            try
            {
                var registro = _db.Producto.Where(r => r.Id == ID).FirstOrDefault(); // Consulta el id
                if (registro == null)
                {
                    return BadRequest("Error al Elminar");
                }
                _db.Producto.Remove(registro);//elimina el registro
                await _db.SaveChangesAsync(); //guarda los cambios
                return RedirectToAction("Index");//redirecciona el index
            }
            catch (Exception)
            {

                return BadRequest("Error al Elminar");
            }

        }
        #endregion
    }

}

