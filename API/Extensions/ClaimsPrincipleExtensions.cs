using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst("Username")?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            string result =  user.FindFirst("UserId")?.Value;
            return string.IsNullOrEmpty(result) ?
                0:
                int.Parse(result);
        }
    }
}