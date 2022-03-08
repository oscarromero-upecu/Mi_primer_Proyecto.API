using AccessoData;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Modelos;
using Negocio.Repositorio.IRepositorio;
using Pedidos_YA.API;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConsumoTelefonico.API.Controllers
{
    //Plantilla de un contoller
    [ApiController]
    [Route("[Controller]/[action]")]
    public class CuentaController : ControllerBase //hereda los metodos y procesos 
    {
        //clases que se crean automaticamente en el proyecto de la API 
        private readonly SignInManager<Usuario> _signInManager; //instancia la clase 
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ConfiguracionJWT _configuracionJwt;   //instancia de las opciones  
        private readonly IUsuarioRepositorio _usuarioRepositorio; //instancia la clase interfaz del Usuario repositorio

        //constructor
        public CuentaController(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<ConfiguracionJWT> opciones, //Ioptions entrega el valor que se define en Programs.cs del JWT
            IUsuarioRepositorio usuarioRepositorio)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _usuarioRepositorio = usuarioRepositorio;
            _configuracionJwt = opciones.Value;
        }

        [Authorize]
        //crear nuevo usuario
        [HttpPost]
        public async Task<IActionResult> RegistrarUsuario([FromBody] RegistroUsuarioRequestDTO registroUsuarioRequestDTO) // etiqueta FromBody que dice que tome este objeto DTO lo va a recibir desde el cuerpo de la peticion
        {
            if (registroUsuarioRequestDTO is null || !ModelState.IsValid)
                return BadRequest(); //retorna que es una solicitud mala
            //nuevo usuario
            var usuario = new Usuario
            {
                UserName = registroUsuarioRequestDTO.Email,
                Email = registroUsuarioRequestDTO.Email,
                NombreUsuario = registroUsuarioRequestDTO.Nombre,
                EmailConfirmed = true
            };

            var resultadoCreacion = await _userManager.CreateAsync(usuario, registroUsuarioRequestDTO.Contrasena);//variable con el metodo  CreateAsync al suaurio y la contrasena

            if (!resultadoCreacion.Succeeded) //valida si la creacion fue satisfactorio (logrado)
                return BadRequest(new ResgistroUsuarioResponseDTO //si no es stisfactorio ponemos un mala solictud 
                {
                    ResgistroSatisfactorio = false,
                    Errores = resultadoCreacion.Errors.Select(error => error.Description) //linkqui de ete error consulta a la base de datos que descriva lista de errores
                });

            return Ok(new ResgistroUsuarioResponseDTO { ResgistroSatisfactorio = true });
        }
        //Crear para iniciar sesion
        [HttpPost]
        public async Task<IActionResult> IniciarSesion([FromBody] InicioSesionRequestDTO inicioSesionRequestDTO) //al objeto solicitud
        {
            if (inicioSesionRequestDTO is null || !ModelState.IsValid)
                return BadRequest(); //estas mal
            // variable para guardar 
            var resultadoInicioSesion = await _signInManager.PasswordSignInAsync(inicioSesionRequestDTO.Correo, inicioSesionRequestDTO.Contrasena, false, false);

            if (resultadoInicioSesion.Succeeded)// si es satisfactorio
            { 
                var usuario = await _userManager.FindByNameAsync(inicioSesionRequestDTO.Correo);//obtener usuario
                if (usuario == null) // si el suario que encuentra esta vacio
                    return Unauthorized(new InicioSesionResponseDTO // respuesta si no es satisfactoria
                    {
                        AutenticaionExitosa = false,
                        MensajeError = "Error de autenticacion"
                    });
                //con la llave privada secreto JWT
                var credencialesInicioSesion = ObtenerCredencialesInicioSesion(); //Metodo privado
                //crear los claims (permisos)
                var claims = await ObtenerClaims(usuario);//metodo privado

                //vamos a crear las opciones del token
                var opcionesToken = new JwtSecurityToken(
                    claims: claims, //con los : se tomalos contados
                    expires: DateTime.Now.AddDays(_configuracionJwt.DiasValidez), //fecha de expiracion del token
                    signingCredentials: credencialesInicioSesion);//agregar las credenciales

                var token = new JwtSecurityTokenHandler().WriteToken(opcionesToken);//crea el token

                return Ok(new InicioSesionResponseDTO //devolver la respuesta del inicio de secion
                {
                    AutenticaionExitosa = true,
                    Token = token,
                    Usuario = new UsuarioDTO
                    {
                        Id = usuario.Id.ToString(),
                        NombreUsuario = usuario.NombreUsuario,
                    }
                });
            }

            return Unauthorized(new InicioSesionResponseDTO
            {
                AutenticaionExitosa = false,
                MensajeError = "Error de autenticacion"
            });
        }

        [Authorize]
        [HttpPut("idUsuario")]
        public async Task<IActionResult> ConvertirAdministrador(string Idusuario)
        {
            var usuario = await _userManager.FindByIdAsync(Idusuario);
            if (usuario == null)
                return BadRequest();

            _userManager.AddToRoleAsync(usuario, Roles.Administrador).GetAwaiter().GetResult();

            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            return Ok(await _usuarioRepositorio.ObtenerUsuarios());
        }

        //vamos a incriptar el secreto (llave)
        private SigningCredentials ObtenerCredencialesInicioSesion()
        {
            var secreto = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracionJwt.Secreto));
            return new SigningCredentials(secreto, SecurityAlgorithms.HmacSha256);
        }

        //devuelve una tarea lista claim
        private async Task<List<Claim>> ObtenerClaims(Usuario usuario)
        {
            //lista de claim, la informacion que quiero que gurade el token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,usuario.NombreUsuario),//tipo de claim por default
                new Claim(ClaimTypes.Email,usuario.Email),
                new Claim("Id",usuario.Id),             //tipo de claim personalizado     
            };

            //vamos agregar los roles
            var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(usuario.Email));//
            foreach (var rol in roles)//a cada uno de los roles le ponga una lista de claims (reclamos)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            return claims; //devolver la lista
        }
    }
}