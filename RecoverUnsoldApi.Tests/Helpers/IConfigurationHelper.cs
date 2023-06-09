using Microsoft.Extensions.Configuration;

namespace RecoverUnsoldApi.Tests.Helpers;

public class ConfigurationHelper
{
    private Dictionary<string, string> _dictionary = new();

    public ConfigurationHelper()
    {
    }

    public ConfigurationHelper With(string key, string value)
    {
        _dictionary.Add(key, value);
        return this;
    }

    public IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(_dictionary)
            .Build();
    }
}