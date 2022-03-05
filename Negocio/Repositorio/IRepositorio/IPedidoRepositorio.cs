using Modelos;

namespace Negocio.Repositorio.IRepositorio
{
    //Interfaz de almacenanimeto (repositorio) DTO
    public interface IPedidoRepositorio
    {
        //Crea una tarea que actue como interfaz para registrar en el objeto plano DTO (ResgistroPedidoDTO)  
        Task<ResgistroPedidoDTO> RegistrarPedido(ResgistroPedidoDTO PedidoDTO);

        //Crea una tarea para que realice una lista (IEnumerable<>) del objeto plano DTO (ResgistroPedidoDTO)
        Task<IEnumerable<ResgistroPedidoDTO>> VerRegistroPedido();

        //Tarea para que realice una lista (IEnumerable<>) del objeto plano DTO (ResgistroPedidoDTO) que obtenga el idUsuario
        Task<IEnumerable<ResgistroPedidoDTO>> VerRegistroPedido(string idUsuario);

        //Tarea para que realice una lista (IEnumerable<>) del objeto plano DTO (ConsumoPorUsuarioDTO) 
        Task<IEnumerable<ConsumoPorUsuarioDTO>> VerConsumoPorUsuario();
    }
}
