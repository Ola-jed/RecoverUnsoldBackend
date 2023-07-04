using Microsoft.Extensions.Configuration;

namespace RecoverUnsoldApi.Tests.Helpers;

public class ConfigurationHelper
{
    private readonly List<KeyValuePair<string, string?>> _dictionary = new();

    public ConfigurationHelper With(string key, string value)
    {
        _dictionary.Add(new KeyValuePair<string, string?>(key, value));
        return this;
    }

    public IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(_dictionary)
            .Build();
    }
}