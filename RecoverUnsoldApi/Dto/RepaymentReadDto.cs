namespace RecoverUnsoldApi.Dto;

public record RepaymentReadDto(Guid Id, bool Done, string? Note, string? TransactionId, OrderReadDto Order,
    DateTime CreatedAt);