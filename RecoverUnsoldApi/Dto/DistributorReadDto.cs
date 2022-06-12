namespace RecoverUnsoldApi.Dto;

public record DistributorReadDto(string Username, string Email, string Phone, string TaxId,
    string Rccm, string? WebsiteUrl, DateTime? EmailVerifiedAt, DateTime CreatedAt) : UserReadDto(Username, Email,
    EmailVerifiedAt, CreatedAt);