using AccessoData;
using Modelos;

namespace Negocio.Repositorio.IRepositorio
{
    //Interfaz de almacenanimeto (repositorio) DTO
    public interface IPedidoRepositorio
    {
        //Crea una tarea que actue como interfaz para registrar en el objeto plano DTO (ResgistroPedidoDTO)  
        //Task<ResgistroPedidoDTO> RegistrarPedido(RegistroPedidoRequestDTO registroPedidoRequestDTO);

        //Crea una tarea para que realice una lista (IEnumerable<>) del objeto plano DTO (ResgistroPedidoDTO)
        Task<IEnumerable<ResgistroPedidoDTO>> VerRegistroPedido();
        Task<IEnumerable<RegistroPedido>> RegistrarPedido (RegistroPedidoRequestDTO requestDTO);

        //Tarea para que realice una lista (IEnumerable<>) del objeto plano DTO (ResgistroPedidoDTO) que obtenga el idUsuario
        Task<IEnumerable<ResgistroPedidoDTO>> VerRegistroPedido(string idUsuario);
        Task<IEnumerable<RegistroPedido>> EliminarPedido(int idPdedido);

        //Tarea para que realice una lista (IEnumerable<>) del objeto plano DTO (ConsumoPorUsuarioDTO) 
        Task<IEnumerable<ConsumoPorUsuarioDTO>> VerConsumoPorUsuario();
    }
}
