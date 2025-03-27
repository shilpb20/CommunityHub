using AutoMapper;
using CommunityHub.Core.Dtos;
using CommunityHub.Infrastructure.Models;
using Newtonsoft.Json;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<UserInfoCreateDto, UserInfo>().ReverseMap();
        CreateMap<UserInfoDto, UserInfo>().ReverseMap();
        CreateMap<UserInfoDto, UserInfoCreateDto>().ReverseMap();

        CreateMap<UserInfoCreateDto, UserContactDto>().ReverseMap();
        CreateMap<SpouseInfoCreateDto, SpouseInfo>().ReverseMap();
        CreateMap<SpouseInfoDto, SpouseInfo>().ReverseMap();
        CreateMap<SpouseInfoDto, SpouseInfoCreateDto>().ReverseMap();

        CreateMap<Children, ChildrenDto>().ReverseMap();
        CreateMap<Children, ChildrenCreateDto>().ReverseMap();
        CreateMap<ChildrenDto, ChildrenCreateDto>().ReverseMap();

        CreateMap<FamilyPicture, FamilyPictureDto>().ReverseMap();
        CreateMap<FamilyPicture, FamilyPictureCreateDto>().ReverseMap();
        CreateMap<FamilyPicture, FamilyPictureUpdateDto>().ReverseMap();
        CreateMap<FamilyPictureDto, FamilyPictureCreateDto>().ReverseMap();

        CreateMap<RegistrationInfoCreateDto, RegistrationInfoDto>().ReverseMap();

        CreateMap<RegistrationInfo, RegistrationInfoDto>().ReverseMap();
        CreateMap<RegistrationInfo, RegistrationInfoCreateDto>().ReverseMap();

        CreateMap<RegistrationRequest, RegistrationRequestDto>()
            .ForMember(dest => dest.RegistrationInfo,
                f => f.MapFrom(src => string.IsNullOrEmpty(src.RegistrationInfo)
                    ? null : JsonConvert.DeserializeObject<RegistrationInfoDto>(src.RegistrationInfo)))
            .ForMember(dest => dest.CreatedAt, f => f.MapFrom(src => src.CreatedAt));

        CreateMap<RegistrationRequestDto, RegistrationRequest>()
            .ForMember(dest => dest.RegistrationInfo,
                f => f.MapFrom(src => src.RegistrationInfo == null
                    ? null : JsonConvert.SerializeObject(src.RegistrationInfo)))
            .ForMember(dest => dest.CreatedAt, f => f.MapFrom(src => src.CreatedAt));
    }
}
