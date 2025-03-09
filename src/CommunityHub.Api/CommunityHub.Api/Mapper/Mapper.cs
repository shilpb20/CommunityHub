using AutoMapper;
using CommunityHub.Core.Dtos.RegistrationData;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;
using Newtonsoft.Json;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<RegistrationData, RegistrationDataDto>().ReverseMap();
        CreateMap<RegistrationData, RegistrationDataCreateDto>().ReverseMap();

        CreateMap<RegistrationRequest, RegistrationRequestDto>()
           .ForMember(dest => dest.RegistrationData, opt =>
               opt.MapFrom(src =>
                   string.IsNullOrEmpty(src.RegistrationData)
                       ? null
                       : JsonConvert.DeserializeObject<RegistrationDataDto>(src.RegistrationData)))
           .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<RegistrationRequestDto, RegistrationRequest>()
            .ForMember(dest => dest.RegistrationData, opt =>
                opt.MapFrom(src =>
                    src.RegistrationData == null
                        ? null
                        : JsonConvert.SerializeObject(src.RegistrationData)))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreateAt));
    }
}
