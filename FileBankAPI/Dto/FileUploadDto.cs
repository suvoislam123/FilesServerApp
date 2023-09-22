using System.ComponentModel.DataAnnotations;

namespace FileBankAPI.Dto
{
    public class FileUploadDto
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }    
    }
}
