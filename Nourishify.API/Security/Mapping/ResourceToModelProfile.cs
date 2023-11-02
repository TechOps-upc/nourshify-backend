using AutoMapper;
using Nourishify.API.Security.Domain.Models;
using Nourishify.API.Security.Domain.Services.Communication;
using Nourishify.API.Security.Resources;

namespace Nourishify.API.Security.Mapping;

public class ResourceToModelProfile : Profile
{
    public ResourceToModelProfile()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<SaveRoleResource, Role>();
        CreateMap<UpdateRequest, User>()
            .ForAllMembers(options => options.Condition(
                (source, target, property) =>
                {
                    if (property == null) return false;
                    if (property.GetType() == typeof(string) && string.IsNullOrEmpty((string)property)) return false;
                    return true;
                }
            ));
    }
}