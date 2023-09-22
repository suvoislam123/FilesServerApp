using AutoMapper;
using Core.Entities;
using FileBankAPI.Dto;

namespace FileBankAPI.Helpers
{
    public class FileUrlResolver : IValueResolver<FileUpload, FileUploadToReturnDto,string>
    {
        private readonly IConfiguration _config;
        public  FileUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(FileUpload source, FileUploadToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.FileUrl))
            {
                return _config["ApiUrl"]+source.FileUrl;
            }
            return null;
        }
    }
}
