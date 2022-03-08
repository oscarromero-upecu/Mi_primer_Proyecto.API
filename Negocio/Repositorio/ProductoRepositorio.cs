using AccessoData;
using AccessoData.Contexto;
using AutoMapper;
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
    }
}
