using System.ComponentModel.DataAnnotations;
using RecoverUnsoldDomain.Data;

namespace RecoverUnsoldApi.Validation;

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value,
        ValidationContext validationContext)
    {
        var context = validationContext.GetService<DataContext>()!;
        return context.Users.Any(x => x.Email == value!.ToString())
            ? new ValidationResult("Email address already in use")
            : ValidationResult.Success;
    }
}