namespace Pedidos_YA.API
{
    //Clase que entraga o recibe el tokem secreto para luego ser llamado en program
    public class ConfiguracionJWT
    {
        public string Secreto { get; set; }
        public int DiasValidez { get; set; }
    }
}
