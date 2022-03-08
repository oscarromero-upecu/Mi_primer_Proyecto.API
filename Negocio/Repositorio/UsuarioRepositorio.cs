using AccessoData.Contexto;
using AutoMapper;
using Modelos;
using Negocio.Repositorio.IRepositorio;

namespace Negocio.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        //crea un objeto de la base de datos que solo sea visible y privado
        private readonly AppDbContext _db;
        //crea un objeto con las funciones de mapeo que solo sea visible y privado
        private readonly IMapper _mapper;

        //Contructor es el iniciador de un objeto a partir de ua clase
        //Contructor de pedidos repositorios que recibe un objeto (base de datos) y un objeto (mapper)
        public UsuarioRepositorio(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UsuarioDTO>> ObtenerUsuarios()
        {
            return _mapper.Map<IEnumerable<UsuarioDTO>>(_db.Usuario);
        }
    }
}
