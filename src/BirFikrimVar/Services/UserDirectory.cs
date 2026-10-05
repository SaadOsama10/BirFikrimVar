using System.Security.Claims;
using BirFikrimVar.DAL;
using Microsoft.AspNetCore.Identity;

namespace BirFikrimVar.Services
{
    /// <summary>Replaces the static UserHelper of the MVC 5 version: current user id and cached id -> user name lookups.</summary>
    public interface IUserDirectory
    {
        string CurrentUserId { get; }
        bool IsAdmin { get; }
        string GetUserName(string userId);
    }

    public class UserDirectory : IUserDirectory
    {
        private readonly IHttpContextAccessor accessor;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Dictionary<string, string> cache = new Dictionary<string, string>();

        public UserDirectory(IHttpContextAccessor accessor, UserManager<ApplicationUser> userManager)
        {
            this.accessor = accessor;
            this.userManager = userManager;
        }

        public string CurrentUserId =>
            accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public bool IsAdmin => accessor.HttpContext?.User?.IsInRole("Admin") ?? false;

        public string GetUserName(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return "";
            if (!cache.TryGetValue(userId, out var name))
            {
                name = userManager.Users.Where(u => u.Id == userId).Select(u => u.UserName).FirstOrDefault() ?? "";
                cache[userId] = name;
            }
            return name;
        }
    }
}
