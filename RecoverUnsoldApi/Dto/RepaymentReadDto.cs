namespace RecoverUnsoldApi.Dto;

public record RepaymentReadDto(bool Done, string? Note, string? TransactionId, OrderReadDto Order, DateTime CreatedAt);