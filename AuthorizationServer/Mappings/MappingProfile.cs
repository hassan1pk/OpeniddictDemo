using AuthorizationServer.Models;
using AutoMapper;

namespace AuthorizationServer.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<UserRegistrationModel, ApplicationUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
