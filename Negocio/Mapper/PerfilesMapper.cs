using AccessoData;
using AutoMapper;
using Modelos;

namespace Negocio.Mapper
{
    //Modelo de programacion para mapear ORM
    //vinculo entre DB y DTO para que le CRUD (crear,leer,actualizar,borrar) pueda relizarce de manera directa
    public class PerfilesMapper : Profile //hereda perfiles preestablecidos de mapeo
    {
        //metodo donde crear el mapa y permite vincular las entidades virtuales con en objeto plano DTO y viceversa
        public PerfilesMapper()
        {
            
            CreateMap<RegistroPedido, ResgistroPedidoDTO>().ReverseMap();
            CreateMap<Producto, ProductoDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
        }
    }
}
