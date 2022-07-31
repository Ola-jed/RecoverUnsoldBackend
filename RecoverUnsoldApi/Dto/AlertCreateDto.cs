using System.ComponentModel.DataAnnotations;
using RecoverUnsoldApi.Entities.Enums;

namespace RecoverUnsoldApi.Dto;
// TODO : validate that the distributor id really exists
public record AlertCreateDto([Required] AlertType AlertType, Guid? DistributorId = null);