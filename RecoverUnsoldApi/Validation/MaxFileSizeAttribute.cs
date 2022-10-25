using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Validation;

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
        switch (value)
        {
            case null when Nullable:
                return ValidationResult.Success;
            case IEnumerable<IFormFile> values:
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