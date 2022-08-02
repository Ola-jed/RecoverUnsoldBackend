using System.ComponentModel.DataAnnotations;

namespace RecoverUnsoldApi.Dto;

public record OpinionCreateDto([Required] [Range(0, 5)] int Rating, [Required] string Comment);