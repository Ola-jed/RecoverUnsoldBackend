namespace RecoverUnsoldApi.Dto;

public record DistributorReadDto(Guid Id, string Username, string Email, string Phone, string TaxId,
    string Rccm, string? WebsiteUrl, DateTime? EmailVerifiedAt, DateTime CreatedAt) : UserReadDto(Id, Username, Email,
    EmailVerifiedAt, CreatedAt);