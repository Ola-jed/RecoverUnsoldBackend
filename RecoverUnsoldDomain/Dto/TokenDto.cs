namespace RecoverUnsoldDomain.Dto;

public record TokenDto(string Token, string? Role, DateTime ExpirationDate);