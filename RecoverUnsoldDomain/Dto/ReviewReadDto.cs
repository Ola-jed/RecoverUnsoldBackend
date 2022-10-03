namespace RecoverUnsoldDomain.Dto;

public record ReviewReadDto(string Comment, UserReadDto User, DateTime CreatedAt);