namespace RecoverUnsoldApi.Dto;

public record DistributorReadDto(string Username, string Email, string Phone, string Ifu,
    string Rccm, string? WebsiteUrl, DateTime? EmailVerifiedAt, DateTime CreatedAt) : UserReadDto(Username, Email,
    EmailVerifiedAt, CreatedAt);