using System.Text;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldAdmin.Extensions;

public static class EntitiesExtensions
{
    public static string Fullname(this Customer self)
    {
        return self.FirstName == null
            ? self.LastName ?? "-"
            : $"{self.FirstName} {self.LastName ?? ""}";
    }
}