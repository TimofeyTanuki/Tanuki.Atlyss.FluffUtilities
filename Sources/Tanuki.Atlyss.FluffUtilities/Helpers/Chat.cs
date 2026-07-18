using Tanuki.Atlyss.FluffUtilities.Extensions;

namespace Tanuki.Atlyss.FluffUtilities.Helpers;

internal sealed class Chat
{
    public static void AddTranslatedMessage(string translationKey, params object[] placeholder) =>
        Core.Tanuki.Instance.Managers.Chat.AddMessage(Main.Translate(translationKey, placeholder));
}
