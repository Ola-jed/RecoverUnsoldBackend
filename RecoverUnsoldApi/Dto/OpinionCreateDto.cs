using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record OpinionCreateDto([Required] string Comment);