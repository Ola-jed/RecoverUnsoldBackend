namespace RecoverUnsoldAdmin.Services.Stats;

public interface IStatsService
{
    Task<Models.Stats> GetStats();
}