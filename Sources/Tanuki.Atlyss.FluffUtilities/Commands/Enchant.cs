using System;
using System.Collections.Generic;
using Tanuki.Atlyss.API.Collections;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.Game.Extensions;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Enchant : ICommand
{
    private static readonly Core.Managers.Chat chatManager;
    private static readonly TranslationSet translationSet;

    static Enchant()
    {
        chatManager = Core.Tanuki.Instance.Managers.Chat;
        translationSet = Main.Instance.translationSet;
    }

    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;
        Player player = Player._mainPlayer;

        ItemData? itemData = player._pInventory.GetItem(0, false, ItemType.GEAR);
        if (itemData is null)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.Enchant.FirstEquipmentSlotIsEmpty"));
            return;
        }

        ScriptableItem scriptableItem = GameManager._current.Locate_Item(itemData._itemName);
        ScriptableEquipment scriptableEquipment = (ScriptableEquipment)scriptableItem;

        if (!scriptableEquipment._statModifierTable)
        {
            chatManager.SendClientMessage(translationSet.Translate("Commands.Enchant.NotEnchantableItem", itemData._itemName));
            return;
        }

        byte modifier = (byte)(scriptableEquipment.GetType() == typeof(ScriptableWeapon) ? 1 : 0);

        if (arguments.Count == 0)
        {
            if (modifier == 0)
                DisplayModifiers(chatManager, translationSet, scriptableEquipment._statModifierTable);
            else
                DisplayDamageTypes(chatManager, translationSet);

            return;
        }

        bool useDamageTypeOverride = itemData._useDamageTypeOverride;
        DamageType damageType = itemData._damageTypeOverride;
        int modifierId = itemData._modifierID;

        if (modifier > 0)
        {
            if (!ushort.TryParse(arguments[0], out ushort damageTypeIndex))
            {
                chatManager.SendClientMessage(Main.Instance.Translate("Commands.Enchant.DamageTypeNotInteger"));
                return;
            }

            if (damageTypeIndex >= Enum.GetNames(typeof(DamageType)).Length)
            {
                DisplayDamageTypes(chatManager, translationSet);
                return;
            }

            damageType = (DamageType)damageTypeIndex;
            useDamageTypeOverride = true;
        }

        if (arguments.Count > modifier)
        {
            if (!int.TryParse(arguments[modifier], out modifierId))
            {
                chatManager.SendClientMessage(translationSet.Translate("Commands.Enchant.ModifierNotInteger"));
                return;
            }

            ScriptableStatModifier? scriptableStatModifier = null;

            foreach (StatModifierSlot statModifierSlot in scriptableEquipment._statModifierTable._statModifierSlots)
            {
                if (statModifierSlot._equipModifier._modifierID != modifierId)
                    continue;

                scriptableStatModifier = statModifierSlot._equipModifier;
                break;
            }

            if (!scriptableStatModifier)
            {
                DisplayModifiers(chatManager, translationSet, scriptableEquipment._statModifierTable);
                return;
            }

            modifierId = scriptableStatModifier!._modifierID;
        }

        itemData._damageTypeOverride = damageType;
        itemData._useDamageTypeOverride = useDamageTypeOverride;
        itemData._modifierID = modifierId;

        player._pSound._aSrcGeneral.PlayOneShot(player._pSound._purchaseItemSound);
    }

    private void DisplayDamageTypes(Core.Managers.Chat chatManager, TranslationSet translationSet)
    {
        Type type = typeof(DamageType);
        List<string> damageTypeNames = [];

        byte value = 0;
        foreach (string name in Enum.GetNames(type))
        {
            damageTypeNames.Add(translationSet.Translate("Commands.Enchant.DamageTypes.DamageType", value, name));
            value++;
        }

        chatManager.SendClientMessage(
            translationSet.Translate(
                "Commands.Enchant.DamageTypes",
                string.Join(
                    translationSet.Translate("Commands.Enchant.DamageTypes.Separator"),
                    damageTypeNames
                )
            )
        );
    }

    private void DisplayModifiers(Core.Managers.Chat chatManager, TranslationSet translationSet, ScriptableStatModifierTable scriptableStatModifierTable)
    {
        List<string> modifierNames = [];

        foreach (StatModifierSlot statModifierSlot in scriptableStatModifierTable._statModifierSlots)
            modifierNames.Add(
                translationSet.Translate(
                    "Commands.Enchant.Modifiers.Modifier",
                    statModifierSlot._equipModifier._modifierID,
                    statModifierSlot._equipModifier._modifierTag
                )
            );

        chatManager.SendClientMessage(
            translationSet.Translate(
                "Commands.Enchant.Modifiers",
                string.Join(
                    translationSet.Translate("Commands.Enchant.Modifiers.Separator"),
                    modifierNames
                )
            )
        );
    }
}
