using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record OpinionUpdateDto([Required] string Comment);