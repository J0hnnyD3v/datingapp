using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions
{
  public static string GetEmail(this ClaimsPrincipal user) => user.FindFirst(ClaimTypes.Email)?.Value;
}
