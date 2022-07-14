namespace RecoverUnsoldApi.Dto;

public record OpinionReadDto(Guid Id, int Rating, string Comment, Guid OrderId);