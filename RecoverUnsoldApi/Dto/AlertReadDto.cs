using RecoverUnsoldApi.Entities.Enums;

namespace RecoverUnsoldApi.Dto;

public record AlertReadDto(AlertType AlertType, DistributorInformationDto? DistributorInformationDto = null);