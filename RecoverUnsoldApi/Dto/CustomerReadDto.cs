namespace RecoverUnsoldApi.Dto;

public record CustomerReadDto(Guid Id,string Username, string Email, string? FirstName, string? LastName,
    DateTime? EmailVerifiedAt, DateTime CreatedAt) : UserReadDto(Id, Username, Email, EmailVerifiedAt, CreatedAt);