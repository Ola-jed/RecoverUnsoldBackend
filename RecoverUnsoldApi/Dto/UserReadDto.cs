namespace RecoverUnsoldApi.Dto;

public abstract record UserReadDto(string Username, string Email, DateTime? EmailVerifiedAt, DateTime CreatedAt);