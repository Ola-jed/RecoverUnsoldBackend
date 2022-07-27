namespace RecoverUnsoldApi.Dto;

public record DistributorFilterDto(int Page = 1, int PerPage = 10, string? Name = null);