using System;
using Tanuki.Atlyss.API.Commands;
using Tanuki.Atlyss.Game.Extensions;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class Enchant : ICommand
{
    public bool Execute(string[] Arguments)
    {
        if (Arguments.Length != 0)
        {
            ItemData ItemData = Player._mainPlayer._pEquipment.UsableWeapon()._heldItem;

            if (ItemData is null)
            {
                ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Enchant.NoUsableWeapon"));
                return false;
            }

            if (Enum.TryParse(Arguments[0], true, out DamageType DamageType))
            {
                ItemData._damageTypeOverride = DamageType;
                ItemData._useDamageTypeOverride = true;
                Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._lockonSound);
                return false;
            }
        }

        ChatBehaviour._current.New_ChatMessage(
            Main.Instance.Translate(
                "Commands.Enchant.DamageTypes",
                string.Join(
                    Main.Instance.Translate("Commands.Enchant.DamageTypes.Separator"),
                    Enum.GetNames(typeof(DamageType))
                )
            )
        );

        return false;
    }
}