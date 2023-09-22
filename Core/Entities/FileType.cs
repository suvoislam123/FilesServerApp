using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FileType : BaseEntity
    {
        public string TypeName { get; set; }    
        /*public ICollection<FileUpload>? FileUploads { get; set; }*/
    }
}
