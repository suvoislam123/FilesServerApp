using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class FilesWithTypeSpecification : BaseSpecification<FileUpload>
    {
        public FilesWithTypeSpecification(FileSpecParams specParams)
            : base(file =>
                (string.IsNullOrEmpty(specParams.Search) || file.Name.ToLower().Contains(specParams.Search)) &&
                (!specParams.TypeId.HasValue || file.FileTypeId == specParams.TypeId) 
                )
        {
            AddInclude(file => file.FileType);
            AddOrderBy(file => file.Name);
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "sizeAsc":
                        AddOrderBy(f => f.FileSize);
                        break;
                    case "sizeDesc":
                        AddOrderByDescending(f => f.FileSize);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }

        }
        public FilesWithTypeSpecification(int id):base(file=>file.Id == id)
        {
            AddInclude(file => file.FileType);
        }
    }
}
