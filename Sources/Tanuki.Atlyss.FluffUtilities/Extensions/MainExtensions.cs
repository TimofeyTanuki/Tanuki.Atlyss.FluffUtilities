namespace Tanuki.Atlyss.FluffUtilities.Extensions;

public static class MainExtensions
{
    extension(Main)
    {
        public static string Translate(string key, params object[] placeholder) => Main.Instance.Translate(key, placeholder);
    }
}
