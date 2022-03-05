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

        //Contructor es el iniciador de un objeto a partir de ua clase
        //Contructor de pedidos repositorios que recibe un objeto (base de datos) y un objeto (mapper)
        public PedidosRepositorio(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        //Tarea asincronica para registrar un pedido en la base de datos con parametro de objeto DTO (PedidoDTO)
        public async Task<ResgistroPedidoDTO> RegistrarPedido(ResgistroPedidoDTO PedidoDTO)
        {
            //mapea como "pedido" desde el resgristroPedidoDTO "PedidoDTO" hacia la base de datos, entidad "RegistroPedido" 
            var pedido = _mapper.Map<RegistroPedido>(PedidoDTO);

            //luego como "nuevoPedido" agrega a la base de datos de "RegistroPedido" el registroPedidosDTO "PedidoDTO"
            var nuevoPedido = _db.RegistroPedido.Add(pedido);

            //await(esperar) es el break para la tarea y guarda los cambios asincronicos en la base de datos
            await _db.SaveChangesAsync();

            //retorna la entidad creada en la base de datos convertida a DTO
            return _mapper.Map<ResgistroPedidoDTO>(nuevoPedido.Entity);
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
            var listaPedidos = _db.RegistroPedido.Where(pedido => pedido.UserId == idUsuario);

            //retorna una lista (IEnumerable<>) en DTO (ResgistroPedidoDTO) la consulta del ID de Usuario
            return _mapper.Map<IEnumerable<ResgistroPedidoDTO>>(listaPedidos);
        }

        //Tarea para que retorne la consulta por grupo desde la base de datos en (IEnumerable<>) convertida a DTO   
        public async Task<IEnumerable<ConsumoPorUsuarioDTO>> VerConsumoPorUsuario()
        {
            return _db.RegistroPedido.GroupBy(registro => new
            {
                registro.UserId,
                registro.Usuario.Nombre,
                registro.Usuario.DescuentoPedido,
            }).Select(grupo => new ConsumoPorUsuarioDTO
            {
                IdUsuario = grupo.Key.UserId,
                Nombre = grupo.Key.Nombre,
                CantidadPedidos = grupo.Count(),
                TotalAPagar = grupo.Sum(registro => registro.PrecioPedido - registro.Usuario.DescuentoPedido)
            });

        }
    }
}
