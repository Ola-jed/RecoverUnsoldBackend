using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Validation;

public class MaxFileSizeAttribute : ValidationAttribute
{
    public uint MaxFileSize { get; set; }
    public bool Nullable { get; set; }

    public MaxFileSizeAttribute(uint maxFileSize, bool nullable = false)
    {
        MaxFileSize = maxFileSize;
        Nullable = nullable;
    }

    protected override ValidationResult? IsValid(object? value,
        ValidationContext validationContext)
    {
        if ((value == null && Nullable))
        {
            return ValidationResult.Success;
        }

        if (value is IEnumerable<IFormFile> values)
        {
            return values.Any(formFile => formFile.Length > MaxFileSize)
                ? new ValidationResult(GetErrorMessage())
                : ValidationResult.Success;
        }

        if (value is not IFormFile file)
        {
            return ValidationResult.Success;
        }

        return file.Length > MaxFileSize
            ? new ValidationResult(GetErrorMessage())
            : ValidationResult.Success;
    }

    private string GetErrorMessage()
    {
        return $"Maximum allowed file size is {MaxFileSize} bytes.";
    }
}