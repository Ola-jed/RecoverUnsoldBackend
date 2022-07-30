using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record ReviewCreateDto([Required] string Comment);