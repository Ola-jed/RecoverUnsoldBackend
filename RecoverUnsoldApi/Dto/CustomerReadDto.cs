namespace RecoverUnsoldApi.Dto;

public record CustomerReadDto(string Username, string Email, string? FirstName, string? LastName,
    DateTime? EmailVerifiedAt, DateTime CreatedAt): UserReadDto(Username,Email,EmailVerifiedAt,CreatedAt);