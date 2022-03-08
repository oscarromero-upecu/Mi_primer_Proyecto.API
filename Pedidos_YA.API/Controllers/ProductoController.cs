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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RegistrarProducto([FromBody] RegistroProductoRequestDTO registroProductoRequestDTO)
        {
            if (registroProductoRequestDTO is null || !ModelState.IsValid)
                return BadRequest(); //retorna que es una solicitud mala
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
                return BadRequest(new ResgistroPedidoResponseDTO //si no es stisfactorio ponemos un mala solictud 
                {
                    ResgistroSatisfactorio = false,
                    Mensaje = "Error al registrar"
                });

            return Ok(new ResgistroPedidoResponseDTO { ResgistroSatisfactorio = true });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> ObtenerProducto()// 
        {

            return Ok(await _productoRepositorio.ObtenerProducto());

        }
    }

    
}
