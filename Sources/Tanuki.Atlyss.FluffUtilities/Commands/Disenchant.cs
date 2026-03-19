using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.Game.Extensions;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Disenchant : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;

    static Disenchant()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;
    }

    public void Execute(IContext context)
    {
        Player player = Player._mainPlayer;
        ItemData? itemData = player._pInventory.GetItem(0, false, ItemType.GEAR);

        if (itemData is null)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.Disenchant.FirstEquipmentSlotIsEmpty"));
            return;
        }

        itemData._modifierID = 0;
        itemData._useDamageTypeOverride = false;

        player._pSound._aSrcGeneral.PlayOneShot(player._pSound._lockonSound);
    }
}
