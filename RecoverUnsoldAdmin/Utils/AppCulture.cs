using System.Globalization;

namespace RecoverUnsoldAdmin.Utils;

public static class AppCulture
{
    public static readonly CultureInfo[] SupportedCultures = { new("en-US"), new("fr-FR") };

    public static string CultureLabel(CultureInfo cultureInfo)
    {
        return cultureInfo.Name switch
        {
            "en-US" => "en",
            "fr-FR" => "fr",
            _       => "_"
        };
    }
}