namespace RecoverUnsoldDomain.Services.FcmTokens;

public interface IFcmTokensService
{
    Task Create(Guid userId, string value);
    Task RemoveAllForUser(Guid userId);
    Task Remove(string value);
}