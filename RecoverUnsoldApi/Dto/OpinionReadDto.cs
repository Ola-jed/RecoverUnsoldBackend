namespace RecoverUnsoldApi.Dto;

public record OpinionReadDto(Guid Id, string Comment, Guid OrderId, DateTime CreatedAt);