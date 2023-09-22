
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;


namespace Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
