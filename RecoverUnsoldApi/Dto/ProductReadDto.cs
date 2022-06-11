namespace RecoverUnsoldApi.Dto;

public record ProductReadDto(Guid Id,string Name,string Description,Guid OfferId,IEnumerable<ImageReadDto> Images,DateTime CreatedAt);