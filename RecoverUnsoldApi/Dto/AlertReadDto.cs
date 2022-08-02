using RecoverUnsoldApi.Entities.Enums;

namespace RecoverUnsoldApi.Dto;

public record AlertReadDto(Guid Id, AlertType AlertType, DistributorInformationDto? DistributorInformation = null);