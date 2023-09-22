using System.ComponentModel.DataAnnotations;

namespace FileBankAPI.Dto
{
    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
