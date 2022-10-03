namespace RecoverUnsoldDomain.Dto;

public record DistributorInformationDto(Guid Id, string Username, string Email, string Phone,
    string? WebsiteUrl, DateTime CreatedAt);