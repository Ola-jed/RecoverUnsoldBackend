using System.ComponentModel.DataAnnotations;
using RecoverUnsoldDomain.Entities.Enums;

namespace RecoverUnsoldApi.Dto;

public record AlertCreateDto([Required] AlertType AlertType, Guid? DistributorId = null);