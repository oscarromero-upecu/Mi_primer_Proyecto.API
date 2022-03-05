using Modelos;

namespace Negocio.Repositorio.IRepositorio
{
    //Interfaz de almacenanimeto (UsuarioDTO) DTO
    public interface IUsuarioRepositorio
    {
        //Crea una tarea que actue como interfaz para obtener en el objeto plano DTO (UsuarioDTO)
        Task<IEnumerable<UsuarioDTO>> ObtenerUsuarios();
    }
}
