namespace RecoverUnsoldApi.Dto;

public record AuthenticationResultDto(string Token, UserDataDto UserData, DateTime ExpirationDate);