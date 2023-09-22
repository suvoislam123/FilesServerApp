using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FileUpload : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileUrl { get; set; }
        [Required]
        public double FileSize { get; set; }
        public FileType FileType { get; set; }
        public int FileTypeId {  get; set; }
        
    }
}
