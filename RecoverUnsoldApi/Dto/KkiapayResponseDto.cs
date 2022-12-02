using System.Text.Json.Serialization;

namespace RecoverUnsoldApi.Dto;

public class KkiapayResponseDto
{
    [JsonPropertyName("status")]
    public required string Status { get; set; } = null!;
}