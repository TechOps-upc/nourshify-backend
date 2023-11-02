using AutoMapper;
using Nourishify.API.Security.Domain.Models;
using Nourishify.API.Security.Domain.Services.Communication;
using Nourishify.API.Security.Resources;

namespace Nourishify.API.Security.Mapping;

public class ModelToResourceProfile : Profile
{
    public ModelToResourceProfile()
    {
        CreateMap<User, AuthenticateResponse>();
        CreateMap<User, UserResource>();
        CreateMap<Role, RoleResource>();
        CreateMap<Role, SaveRoleResource>();
    }
}