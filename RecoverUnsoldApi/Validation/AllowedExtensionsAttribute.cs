using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Validation;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensionsAttribute(string extensions)
    {
        _extensions = extensions.Split(",");
    }

    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not IFormFile file)
        {
            return new ValidationResult("No file detected");
        }

        if (value is IEnumerable<IFormFile> values)
        {
            return values.Any(
                formFile => !_extensions.Contains(Path.GetExtension(formFile.FileName).ToLowerInvariant()))
                ? new ValidationResult(GetErrorMessage())
                : ValidationResult.Success;
        }

        var extension = Path.GetExtension(file.FileName);
        return _extensions.Contains(extension.ToLower())
            ? ValidationResult.Success
            : new ValidationResult(GetErrorMessage());
    }

    private string GetErrorMessage()
    {
        return $"This extension is not allowed, allowed extensions are : {string.Join(",", _extensions)}";
    }
}