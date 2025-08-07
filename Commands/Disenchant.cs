using Tanuki.Atlyss.API.Commands;
using Tanuki.Atlyss.Game.Extensions;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class Disenchant : ICommand
{
    public void Execute(string[] Arguments)
    {
        ItemData ItemData = Player._mainPlayer._pEquipment.UsableWeapon()._heldItem;

        if (ItemData is null)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Disenchant.NoUsableWeapon"));
            return;
        }

        ItemData._useDamageTypeOverride = false;
        Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
    }
}