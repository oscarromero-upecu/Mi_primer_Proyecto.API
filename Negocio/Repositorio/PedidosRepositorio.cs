using AccessoData;
using AccessoData.Contexto;
using AutoMapper;
using Modelos;
using Negocio.Repositorio.IRepositorio;


namespace Negocio.Repositorio

{//Repositorio que hereda la interfaz repostorio  
    public class PedidosRepositorio : IPedidoRepositorio
    {
        //Instancia un objeto de la base de datos que solo sea visible y privado
        private readonly AppDbContext _db;
        //Instancia un objeto con las funciones de mapeo que solo sea visible y privado
        private readonly IMapper _mapper;
        private readonly IProductoRepositorio _productoRepositorio;

        //Contructor es el iniciador de un objeto a partir de ua clase
        //Contructor de pedidos repositorios que recibe un objeto (base de datos) y un objeto (mapper)
        public PedidosRepositorio(AppDbContext db, IMapper mapper, IProductoRepositorio productoRepositorio)
        {
            _db = db;
            _mapper = mapper;
            _productoRepositorio = productoRepositorio;
        }

        //Tarea para que retorne la entidad (RegistroPedido) de la base de datos en tipo lista (IEnumerable<>) convertida a DTO  
        public async Task<IEnumerable<ResgistroPedidoDTO>> VerRegistroPedido()
        {
            return _mapper.Map<IEnumerable<ResgistroPedidoDTO>>(_db.RegistroPedido);            
        }

        //Tarea para que retorne de la entidad (RegistroPedido) el idUsuario de la DB en tipo lista (IEnumerable<>) convertida a DTO   
        public async Task<IEnumerable<ResgistroPedidoDTO>> VerRegistroPedido(string idUsuario)
        {
            //realiza consulta a la base de datos Donde el pedido.UserId == idUsuario para para extraer el ID de Usuario
            var listaPedidos = _db.RegistroPedido.Where(pedido => pedido.UsuarioId == idUsuario);

            //retorna una lista (IEnumerable<>) en DTO (ResgistroPedidoDTO) la consulta del ID de Usuario
            return _mapper.Map<IEnumerable<ResgistroPedidoDTO>>(listaPedidos);
        }

        //Tarea para que retorne la consulta por grupo desde la base de datos en (IEnumerable<>) convertida a DTO   
        public async Task<IEnumerable<ConsumoPorUsuarioDTO>> VerConsumoPorUsuario()
        {
            //cosulta primero por grupo luego selecion
            return _db.RegistroPedido.GroupBy(registro => new
            {
                registro.UsuarioId,
                registro.Usuario.NombreUsuario,
                registro.NombreCliente,
                // al hacer select no necesita agregar el retorno de IEnumerable
            }).Select(grupo => new ConsumoPorUsuarioDTO//objetos dinamicos que  tiene caracteristicas propias
            {
                UsuarioId = grupo.Key.UsuarioId, //key accedde alos datos que agrupas
                NombreUsuario = grupo.Key.NombreUsuario,
                NombreCliente = grupo.Key.NombreCliente,
                CantidadPedidos = grupo.Count(),
                TotalConsumido = grupo.Sum(registro => registro.PrecioPedido - registro.Descuento)
            });

        }

        public async Task<IEnumerable<RegistroPedido>> RegistrarPedido(RegistroPedidoRequestDTO requestDTO)
        {
            
            try
            {
                var productoid = _productoRepositorio.ObtenerIdProducto(requestDTO.NombreProducto).Result;
                if (productoid >= 0)
                {
                    //nuevo Pedido
                    var Pedido = new RegistroPedido
                    {
                        ProductoId = productoid,
                        UsuarioId = requestDTO.UsuarioId,
                        NombreCliente = requestDTO.NombreCliente,
                        NombreProducto = requestDTO.NombreProducto,
                        PrecioPedido = requestDTO.PrecioPedido,
                        Descuento = requestDTO.Descuento,
                        TotalPedido = (requestDTO.PrecioPedido - requestDTO.Descuento),
                        FechaDeRegistro = DateTime.Now,
                    };
                    _db.RegistroPedido.Add(Pedido);
                    await _db.SaveChangesAsync();

                }

                //luego como "nuevoPedido" agrega a la base de datos de "RegistroPedido" el registroPedidosDTO "PedidoDTO"
                return _mapper.Map<IEnumerable<RegistroPedido>>(_db.RegistroPedido);

            }
            catch (Exception)
            {

                return(new List<RegistroPedido>());

            }
        }

        public async Task<IEnumerable<RegistroPedido>> EliminarPedido(int idPdedido)
        {
            try
            {
                var registro = _db.RegistroPedido.Where(r => r.Id == idPdedido).FirstOrDefault(); // Consulta el id
                if (registro == null)
                {
                    return (new List<RegistroPedido>());
                }
                _db.RegistroPedido.Remove(registro);//elimina el registro
               await _db.SaveChangesAsync(); //guarda los cambios
               //luego como "nuevoPedido" agrega a la base de datos de "RegistroPedido" el registroPedidosDTO "PedidoDTO"
                return _mapper.Map<IEnumerable<RegistroPedido>>(_db.RegistroPedido);

            }
            catch (Exception)
            {

                return (new List<RegistroPedido>());
            }
        }
    }
}
