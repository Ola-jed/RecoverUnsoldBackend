namespace RecoverUnsoldApi.Dto;

public record DistributorHomeDto(Dictionary<DateTime, int> OrdersPerDay, IEnumerable<OrderReadDto> Orders);