using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class FilesWithFilterForCountSpecification : BaseSpecification<FileUpload>
    {
        public FilesWithFilterForCountSpecification(FileSpecParams specParams) : base(file =>
                (string.IsNullOrEmpty(specParams.Search) || file.Name.ToLower().Contains(specParams.Search)) &&
                (!specParams.TypeId.HasValue || file.FileTypeId == specParams.TypeId) 
                
                )
        {

        }
    }
}
