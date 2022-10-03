using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Dto;

public record OpinionCreateDto([Required] string Comment);