using System.Text.Json.Serialization;

namespace RecoverUnsoldApi.Dto;

public class KkiapayTransactionDto
{
    [JsonPropertyName("transactionId")]
    public required string TransactionId { get; init; }
}