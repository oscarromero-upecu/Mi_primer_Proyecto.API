using AccessoData;
using Microsoft.AspNetCore.Mvc;
using Modelos;

namespace Negocio.Repositorio.IRepositorio
{
    public interface IProductoRepositorio
    {
        //Task<ProductoDTO> RegistrarProducto(ProductoDTO registroProductoDTO);

        Task<IEnumerable<ProductoDTO>> ObtenerProducto();
        Task<int> ObtenerIdProducto(string NombreProducto);
        Task<IEnumerable<Producto>> EliminarProducto(int idProducto);

        Task<IEnumerable<Producto>> RegistrarProducto(RegistroProductoRequestDTO RequestDTO);
    }
}
