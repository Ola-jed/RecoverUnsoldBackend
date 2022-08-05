namespace RecoverUnsoldApi.Dto;

public record OrdersStatsDto(Dictionary<DateOnly,int> OrdersPerDay);