using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldDomain.Dto;

public record ReviewCreateDto([Required] string Comment);