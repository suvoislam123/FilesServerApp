using AutoMapper;
using Core.Entities;
using FileBankAPI.Dto;

namespace FileBankAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<FileUpload, FileUploadToReturnDto>()
                .ForMember(f => f.FileType, o => o.MapFrom(f => f.FileType.TypeName))
                .ForMember(f => f.FileUrl, o => o.MapFrom<FileUrlResolver>());
        }
    }
}
