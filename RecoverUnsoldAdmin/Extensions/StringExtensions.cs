namespace RecoverUnsoldAdmin.Extensions;

public static class StringExtensions
{
    public static string ToUrl(this string str)
    {
        return str.StartsWith("http") ? str : $"http://{str}";
    }
}