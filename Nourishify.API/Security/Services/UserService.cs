using AutoMapper;
using Nourishify.API.Security.Authorization.Handlers.Interfaces;
using Nourishify.API.Security.Domain.Models;
using Nourishify.API.Security.Domain.Repositories;
using Nourishify.API.Security.Domain.Services;
using Nourishify.API.Security.Domain.Services.Communication;
using Nourishify.API.Security.Exceptions;
using Nourishify.API.Shared.Domain.Repositories;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Nourishify.API.Security.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository; // Interfaz para acceder al repositorio de usuarios
        private readonly IRoleRepository _roleRepository; // Interfaz para acceder al repositorio de roles
        private readonly IUnitOfWork _unitOfWork; // Interfaz para acceder a la unidad de trabajo

        private readonly IJwtHandler _jwtHandler; // Manejador de JWT
        private readonly IMapper _mapper; // Objeto para mapeo de datos

        // Constructor del servicio que recibe las dependencias a través de la inyección de dependencias
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IJwtHandler jwtHandler, IMapper mapper, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        // Método para autenticar a un usuario
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
        {
            var user = await _userRepository.FindByUsernameAsync(request.Username); // Busca al usuario por su nombre de usuario

            // Validación de la autenticación
            if (user == null || !BCryptNet.Verify(request.Password, user.PasswordHash))
            {
                throw new AppException("Username or password is incorrect"); // Lanza una excepción si el usuario no existe o la contraseña es incorrecta
            }

            // Generación del token JWT para la autenticación exitosa
            var response = _mapper.Map<AuthenticateResponse>(user);
            response.Token = _jwtHandler.GenerateToken(user);
            return response;
        }

        // Método para obtener todos los usuarios
        public async Task<IEnumerable<User>> ListAsync()
        {
            return await _userRepository.ListAsync(); // Retorna la lista de usuarios obtenida del repositorio
        }

        // Método para obtener un usuario por su ID
        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _userRepository.FindByIdAsync(id); // Busca al usuario por su ID

            if (user == null)
            {
                throw new KeyNotFoundException("User not found"); // Lanza una excepción si el usuario no existe
            }

            return user;
        }

        public Task<User> GetByIdAsyncV2(int id)
        {
            throw new NotImplementedException();
        }

        // Método para registrar un nuevo usuario
        public async Task RegisterAsync(RegisterRequest request)
        {
            // Validación de la existencia del nombre de usuario
            if (_userRepository.ExistsByUsername(request.Username))
            {
                throw new AppException("Username '" + request.Username + "' is already taken"); // Lanza una excepción si el nombre de usuario ya está en uso
            }

            // Validación de la existencia del rol
            var existingRole = await _roleRepository.FindByIdAsync(request.RoleId);
            if (existingRole == null)
            {
                throw new AppException("Role with id '" + request.RoleId + "' does not exist"); // Lanza una excepción si el rol no existe
            }

            // Mapeo de la solicitud a un nuevo objeto de usuario
            var user = _mapper.Map<User>(request);

            // Hash de la contraseña
            user.PasswordHash = BCryptNet.HashPassword(request.Password);

            // Guardar el usuario
            try
            {
                await _userRepository.AddAsync(user);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception e)
            {
                throw new AppException($"An error occurred while saving the user: {e.Message}"); // Lanza una excepción si ocurre un error al guardar el usuario
            }
        }

        // Método para actualizar un usuario
        public async Task UpdateAsync(int id, UpdateRequest request)
        {
            // Validación de la existencia del usuario
            var user = GetById(id);

            // Validación de la existencia de otro usuario con el mismo nombre de usuario
            var userWithSameName = await _userRepository.FindByUsernameAsync(request.Username);
            if (_userRepository.ExistsByUsername(request.Username) && id != userWithSameName.Id)
            {
                throw new AppException("Username '" + request.Username + "' is already taken"); // Lanza una excepción si otro usuario ya está usando el mismo nombre de usuario
            }

            // Validación de la existencia del rol
            var existingRole = await _roleRepository.FindByIdAsync(request.RoleId);
            if (existingRole == null)
            {
                throw new AppException("Role with id '" + request.RoleId + "' does not exist"); // Lanza una excepción si el rol no existe
            }

            // Hash de la contraseña si se ha ingresado
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = BCryptNet.HashPassword(request.Password);
            }

            // Copiar la solicitud al objeto de usuario y guardar
            _mapper.Map(request, user);
            try
            {
                _userRepository.Update(user);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception e)
            {
                throw new AppException($"An error occurred while updating the user: {e.Message}"); // Lanza una excepción si ocurre un error al actualizar el usuario
            }
        }

        // Método para eliminar un usuario
        public async Task DeleteAsync(int id)
        {
            // Validación de la existencia del usuario
            var user = GetById(id);

            try
            {
                _userRepository.Remove(user);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception e)
            {
                throw new AppException($"An error occurred while deleting the user: {e.Message}"); // Lanza una excepción si ocurre un error al eliminar el usuario
            }
        }

        // Método auxiliar para obtener un usuario por su ID
        private User GetById(int id)
        {
            var user = _userRepository.FindById(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found"); // Lanza una excepción si el usuario no existe
            }
            return user;
        }
    }
}
