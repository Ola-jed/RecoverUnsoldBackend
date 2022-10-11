using System.ComponentModel.DataAnnotations;
using RecoverUnsoldDomain.Data;

namespace RecoverUnsoldApi.Validation;

public class UniqueUsernameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value,
        ValidationContext validationContext)
    {
        var context = validationContext.GetService<DataContext>()!;
        return context.Users.Any(x => x.Username == value!.ToString())
            ? new ValidationResult("Username already in use")
            : ValidationResult.Success;
    }
}