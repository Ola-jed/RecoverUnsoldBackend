using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Validation;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;

    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult? IsValid(object? value,
        ValidationContext validationContext)
    {
        if (value is not IFormFile file)
        {
            return ValidationResult.Success;
        }

        if (value is IEnumerable<IFormFile> values)
        {
            return values.Any(formFile => file.Length > _maxFileSize)
                ? new ValidationResult(GetErrorMessage())
                : ValidationResult.Success;
        }

        return file.Length > _maxFileSize
            ? new ValidationResult(GetErrorMessage())
            : ValidationResult.Success;
    }

    private string GetErrorMessage()
    {
        return $"Maximum allowed file size is {_maxFileSize} bytes.";
    }
}