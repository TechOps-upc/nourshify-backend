using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nourishify.API.Security.Authorization.Attributes;
using Nourishify.API.Security.Domain.Models;
using Nourishify.API.Security.Domain.Services;
using Nourishify.API.Security.Domain.Services.Communication;
using Nourishify.API.Security.Resources;

namespace Nourishify.API.Security.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService; // Interfaz para acceder a los servicios relacionados con los usuarios
        private readonly IMapper _mapper; // Objeto para mapear entre modelos y recursos

        // Constructor del controlador que recibe las dependencias a través de la inyección de dependencias
        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // Método HTTP POST para autenticar a un usuario
        [AllowAnonymous] // Permite el acceso anónimo a este método
        [HttpPost("log-in")]
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var response = await _userService.Authenticate(request); // Autentica al usuario utilizando el servicio correspondiente
            return response; // Devuelve la respuesta de autenticación
        }

        // Método HTTP POST para registrar a un nuevo usuario
        [AllowAnonymous] // Permite el acceso anónimo a este método
        [HttpPost("sign-up")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            await _userService.RegisterAsync(request); // Registra al nuevo usuario utilizando el servicio correspondiente
            return Ok(new { message = "Registration successful" }); // Devuelve una respuesta HTTP 200 OK junto con un mensaje de éxito
        }

        // Método HTTP GET para obtener todos los usuarios
        [AllowAnonymous] // Permite el acceso anonimo a este método
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.ListAsync(); // Obtiene la lista de usuarios desde el servicio
            var resources = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(users); // Mapea los usuarios a los recursos correspondientes
            return Ok(resources); // Devuelve una respuesta HTTP 200 OK junto con los recursos de los usuarios
        }

        // Método HTTP GET para obtener un usuario por su ID
        [AllowAnonymous] // Permite el acceso anonimo a este método
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id); // Obtiene el usuario por su ID utilizando el servicio correspondiente
            var resource = _mapper.Map<User, UserResource>(user); // Mapea el usuario al recurso correspondiente
            return Ok(resource); // Devuelve una respuesta HTTP 200 OK junto con el recurso del usuario
        }

        // Método HTTP PUT para actualizar un usuario por su ID
        [AuthorizeAdmin] // Requiere que el usuario tenga el rol de administrador para acceder a este método
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRequest request)
        {
            await _userService.UpdateAsync(id, request); // Actualiza el usuario por su ID utilizando el servicio correspondiente
            return Ok(new { message = "User updated successfully" }); // Devuelve una respuesta HTTP 200 OK junto con un mensaje de éxito
        }

        // Método HTTP DELETE para eliminar un usuario por su ID
        [AuthorizeAdmin] // Requiere que el usuario tenga el rol de administrador para acceder a este método
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id); // Elimina el usuario por su ID utilizando el servicio correspondiente
            return Ok(new { message = "User deleted successfully" }); // Devuelve una respuesta HTTP 200 OK junto con un mensaje de éxito
        }
    }
}
