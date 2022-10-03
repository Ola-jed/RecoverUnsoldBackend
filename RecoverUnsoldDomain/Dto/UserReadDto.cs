namespace RecoverUnsoldDomain.Dto;

public abstract record UserReadDto(Guid Id,string Username, string Email, DateTime? EmailVerifiedAt, DateTime CreatedAt);