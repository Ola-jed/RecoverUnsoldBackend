using RecoverUnsoldDomain.Entities.Enums;

namespace RecoverUnsoldDomain.Dto;

public record AlertReadDto(Guid Id, AlertType AlertType, DistributorInformationDto? DistributorInformation = null);