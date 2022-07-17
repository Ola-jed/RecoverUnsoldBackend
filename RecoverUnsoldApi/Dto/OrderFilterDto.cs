namespace RecoverUnsoldApi.Dto;

public record OrderFilterDto(int Page = 1, int PerPage = 10,string? Status = null);