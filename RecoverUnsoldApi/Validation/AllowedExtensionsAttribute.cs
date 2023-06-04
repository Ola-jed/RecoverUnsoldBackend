using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Validation;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    public string[] Extensions { get; set; }
    public bool Nullable { get; set; }

    public AllowedExtensionsAttribute(string extensions, bool nullable = false)
    {
        Nullable = nullable;
        Extensions = extensions.Split(",");
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
            case null when Nullable:
                return ValidationResult.Success;
            case IEnumerable<IFormFile> values:
                return values.Any(
                    formFile => !Extensions.Contains(Path.GetExtension(formFile.FileName).ToLowerInvariant()))
                    ? new ValidationResult(GetErrorMessage())
                    : ValidationResult.Success;
        }

        if (value is not IFormFile file)
        {
            return new ValidationResult("No file detected");
        }

        var extension = Path.GetExtension(file.FileName);
        return Extensions.Contains(extension.ToLower())
            ? ValidationResult.Success
            : new ValidationResult(GetErrorMessage());
    }

    private string GetErrorMessage()
    {
        return $"This file extension is not allowed, allowed extensions are : {string.Join(",", Extensions)}";
    }
}