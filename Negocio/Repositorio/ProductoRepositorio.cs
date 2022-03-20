using AccessoData;
using AccessoData.Contexto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Modelos;
using Negocio.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Repositorio
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        //Contructor es el iniciador de un objeto a partir de ua clase
        //Contructor de producto repositorios que recibe un objeto (base de datos) y un objeto (mapper)
        public ProductoRepositorio(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductoDTO>> ObtenerProducto()
        {
            return _mapper.Map<IEnumerable<ProductoDTO>>(_db.Producto);
        }

        public async Task<int> ObtenerIdProducto(string NombreProducto)
        {
            //Consulta a la base de datos para obtener el id de producto
          var productoid = _db.Producto.Where(p => p.NombreProducto == NombreProducto)
                .Select(p => p.Id).FirstOrDefault(); //firstOrdefault obtine el registro en la consulta

            return productoid;
        }

        public async Task<IEnumerable<Producto>> RegistrarProducto (RegistroProductoRequestDTO RequestDTO)
        {
            try
            {
                var Producto = new Producto
                {
                    UsuarioId = RequestDTO.UsuarioId,
                    NombreProducto = RequestDTO.NombreProducto,
                    PrecioProducto = RequestDTO.PrecioProducto,
                    FechaDeRegistro = DateTime.Now,
                };
                  //luego como "nuevoPedido" agrega a la base de datos de "RegistroPedido" el registroPedidosDTO "PedidoDTO"
                _db.Producto.Add(Producto);
                //    //await(esperar) es el break para la tarea y guarda los cambios asincronicos en la base de datos
                await _db.SaveChangesAsync();
                
                //luego como "nuevoPedido" agrega a la base de datos de "RegistroPedido" el registroPedidosDTO "PedidoDTO"
                return _mapper.Map<IEnumerable<Producto>>(_db.Producto);
            }
            catch (Exception)
            {

                return (new List<Producto>());
            }
           
           
        }

        public async Task<IEnumerable<Producto>> EliminarProducto(int idProducto)
        {
            try
            {
                var Producto = _db.Producto.Where(r => r.Id == idProducto).FirstOrDefault(); // Consulta el id
                if (Producto == null)
                {
                    return (new List<Producto>());
                }
                _db.Producto.Remove(Producto);//elimina el registro
                await _db.SaveChangesAsync(); //guarda los cambios
                                              //luego como "nuevoPedido" agrega a la base de datos de "RegistroPedido" el registroPedidosDTO "PedidoDTO"
                return _mapper.Map<IEnumerable<Producto>>(_db.Producto);

            }
            catch (Exception)
            {

                return (new List<Producto>());
            }
        }
    }
}
