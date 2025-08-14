using Tanuki.Atlyss.API.Commands;
using Tanuki.Atlyss.Game.Extensions;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class Disenchant : ICommand
{
    public bool Execute(string[] Arguments)
    {
        ItemData ItemData = Player._mainPlayer._pInventory.GetItem(0, false, ItemType.GEAR);

        if (ItemData is null)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Disenchant.FirstEquipmentSlotIsEmpty"));
            return false;
        }

        ItemData._modifierID = 0;
        ItemData._useDamageTypeOverride = false;
        Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);

        return false;
    }
}