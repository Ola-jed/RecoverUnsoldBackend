using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record ReportCreateDto([Required] string Reason, string Description);