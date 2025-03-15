using AutoMapper;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;
using Newtonsoft.Json;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<UserInfoCreateDto, UserInfo>().ReverseMap();
        CreateMap<UserInfoDto, UserInfo>().ReverseMap();
        CreateMap<UserInfoDto, UserInfoCreateDto>().ReverseMap();

        CreateMap<SpouseInfoCreateDto, SpouseInfo>().ReverseMap();
        CreateMap<SpouseInfoDto, SpouseInfo>().ReverseMap();
        CreateMap<SpouseInfoDto, SpouseInfoCreateDto>().ReverseMap();

        CreateMap<Children, ChildrenDto>().ReverseMap();
        CreateMap<Children, ChildrenCreateDto>().ReverseMap();
        CreateMap<ChildrenDto, ChildrenCreateDto>().ReverseMap();

        CreateMap<RegistrationInfoCreateDto, RegistrationInfoDto>().ReverseMap();

        CreateMap<RegistrationInfo, RegistrationInfoDto>().ReverseMap();
        CreateMap<RegistrationInfo, RegistrationInfoCreateDto>().ReverseMap();

        CreateMap<RegistrationRequest, RegistrationRequestDto>()
            .ForMember(dest => dest.RegistrationInfo,
                f => f.MapFrom(src => string.IsNullOrEmpty(src.RegistrationInfo)
                    ? null : JsonConvert.DeserializeObject<RegistrationInfo>(src.RegistrationInfo)))
            .ForMember(dest => dest.CreatedAt, f => f.MapFrom(src => src.CreatedAt));

        CreateMap<RegistrationRequestDto, RegistrationRequest>()
            .ForMember(dest => dest.RegistrationInfo,
                f => f.MapFrom(src => src.RegistrationInfo == null
                    ? null : JsonConvert.SerializeObject(src.RegistrationInfo)))
            .ForMember(dest => dest.CreatedAt, f => f.MapFrom(src => src.CreatedAt));
    }
}
