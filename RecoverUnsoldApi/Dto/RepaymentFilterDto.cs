namespace RecoverUnsoldApi.Dto;

public record RepaymentFilterDto(int Page = 1, int PerPage = 10, bool? Done = null);