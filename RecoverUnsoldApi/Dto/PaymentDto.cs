namespace RecoverUnsoldApi.Dto;

public record PaymentDto(string TransactionId, Guid OrderId, DateTime CreatedAt);