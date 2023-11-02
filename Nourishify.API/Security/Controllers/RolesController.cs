using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nourishify.API.Security.Domain.Models;
using Nourishify.API.Security.Domain.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Nourishify.API.Security.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [SwaggerTag("Create, read, update and delete Roles")] // Etiqueta Swagger para describir la funcionalidad del controlador
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService; // Interfaz para acceder a los servicios relacionados con los roles
        private readonly IMapper _mapper; // Objeto para mapear entre modelos y recursos

        // Constructor del controlador que recibe las dependencias a través de la inyección de dependencias
        public RolesController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        // Método HTTP GET para obtener todos los roles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.ListAsync(); // Obtiene la lista de roles desde el servicio
            var resources = _mapper.Map<IEnumerable<Role>, IEnumerable<RoleResource>>(roles); // Mapea los roles a los recursos correspondientes
            return Ok(resources); // Devuelve una respuesta HTTP 200 OK junto con los recursos de los roles
        }

        // Método HTTP POST para crear un nuevo rol
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveRoleResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages()); // Si el modelo no es válido, devuelve una respuesta HTTP 400 Bad Request con los mensajes de error del modelo

            var roleModel = _mapper.Map<SaveRoleResource, Role>(resource); // Mapea el recurso del rol al modelo correspondiente

            var roleResponse = await _roleService.SaveAsync(roleModel); // Guarda el nuevo rol utilizando el servicio correspondiente

            if (!roleResponse.Success)
                return BadRequest(roleResponse.Message); // Si no se pudo guardar el rol, devuelve una respuesta HTTP 400 Bad Request con el mensaje de error

            var roleResource = _mapper.Map<Role, RoleResource>(roleResponse.Model); // Mapea el modelo del rol guardado al recurso correspondiente

            return Created(nameof(PostAsync), roleResource); // Devuelve una respuesta HTTP 201 Created junto con el recurso del rol creado y su ubicación en el encabezado de respuesta
        }
    }
}