using System.Security.Claims;

namespace RecoverUnsoldAdmin.Extensions;

public static class ClaimsExtensions
{
    public static string Email(this ClaimsPrincipal self)
    {
        return self.Claims.First(c => c.Type == ClaimTypes.Email).Value;
    }
    
    public static string Name(this ClaimsPrincipal self)
    {
        return self.Claims.First(c => c.Type == ClaimTypes.Name).Value;
    }
}