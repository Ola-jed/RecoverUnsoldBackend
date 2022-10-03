using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Dto;

public record OpinionUpdateDto([Required] string Comment);