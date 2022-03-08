using AccessoData;
using Modelos;

namespace Negocio.Repositorio.IRepositorio
{
    public interface IProductoRepositorio
    {
        //Task<ProductoDTO> RegistrarProducto(ProductoDTO registroProductoDTO);

        Task<IEnumerable<ProductoDTO>> ObtenerProducto();


    }
}
