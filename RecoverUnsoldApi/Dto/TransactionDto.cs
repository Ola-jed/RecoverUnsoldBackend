using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record TransactionDto([Required] string TransactionId);