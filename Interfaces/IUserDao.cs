using System.Security.Claims;

namespace ChatRoomApp.Interfaces
{
    public interface IUserDao
    {
        Task<bool> RegisterUserAsync(string userName, string password);
        Task<bool> UserExistsAsync(string userName);
        ClaimsPrincipal AuthenticateUserAsync(string userName, string password);
    }
}
