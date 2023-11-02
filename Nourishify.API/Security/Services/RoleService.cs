using Nourishify.API.Security.Domain.Models;
using Nourishify.API.Security.Domain.Repositories;
using Nourishify.API.Security.Domain.Services;
using Nourishify.API.Security.Domain.Services.Communication;
using Nourishify.API.Shared.Domain.Repositories;

namespace Nourishify.API.Security.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository; // Interfaz para acceder al repositorio de roles
        private readonly IUnitOfWork _unitOfWork; // Interfaz para acceder a la unidad de trabajo

        // Constructor del servicio que recibe las dependencias a través de la inyección de dependencias
        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        // Método para obtener todos los roles
        public async Task<IEnumerable<Role>> ListAsync()
        {
            return await _roleRepository.ListAsync(); // Retorna la lista de roles obtenida del repositorio
        }

        // Método para guardar un nuevo rol
        public async Task<RoleResponse> SaveAsync(Role role)
        {
            try
            {
                await _roleRepository.AddAsync(role); // Agrega el nuevo rol al repositorio
                await _unitOfWork.CompleteAsync(); // Completa la operación en la unidad de trabajo
                return new RoleResponse(role); // Retorna una respuesta de éxito con el rol guardado
            }
            catch (Exception e)
            {
                return new RoleResponse($"An error occurred while saving the role: {e.Message}"); // Retorna una respuesta de error con el mensaje de excepción
            }
        }
    }
}