using System;
using System.Collections.Generic;
using Tanuki.Atlyss.API.Commands;
using Tanuki.Atlyss.Game.Extensions;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class Enchant : ICommand
{
    public bool Execute(string[] Arguments)
    {
        ItemData ItemData = Player._mainPlayer._pInventory.GetItem(0, false, ItemType.GEAR);
        if (ItemData is null)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Enchant.FirstEquipmentSlotIsEmpty"));
            return false;
        }

        ScriptableItem ScriptableItem = GameManager._current.Locate_Item(ItemData._itemName);
        ScriptableEquipment ScriptableEquipment = (ScriptableEquipment)ScriptableItem;

        if (!ScriptableEquipment._statModifierTable)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Enchant.NotEnchantableItem", ItemData._itemName));
            return false;
        }

        byte ModifierArgument = (byte)(ScriptableEquipment.GetType() == typeof(ScriptableWeapon) ? 1 : 0);

        if (Arguments.Length == 0)
        {
            if (ModifierArgument == 0)
                DisplayModifiers(ScriptableEquipment._statModifierTable);
            else
                DisplayDamageTypes();

            return false;
        }

        bool UseDamageTypeOverride = ItemData._useDamageTypeOverride;
        DamageType DamageType = ItemData._damageTypeOverride;
        int ModifierID = ItemData._modifierID;

        if (ModifierArgument > 0)
        {
            if (!ushort.TryParse(Arguments[0], out ushort DamageTypeIndex))
            {
                ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Enchant.DamageTypeNotInteger"));
                return false;
            }

            if (DamageTypeIndex >= Enum.GetNames(typeof(DamageType)).Length)
            {
                DisplayDamageTypes();
                return false;
            }

            DamageType = (DamageType)DamageTypeIndex;
            UseDamageTypeOverride = true;
        }

        if (Arguments.Length > ModifierArgument)
        {
            if (!int.TryParse(Arguments[ModifierArgument], out ModifierID))
            {
                ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Enchant.ModifierNotInteger"));
                return false;
            }

            ScriptableStatModifier ScriptableStatModifier = null;

            foreach (StatModifierSlot StatModifierSlot in ScriptableEquipment._statModifierTable._statModifierSlots)
            {
                if (StatModifierSlot._equipModifier._modifierID != ModifierID)
                    continue;

                ScriptableStatModifier = StatModifierSlot._equipModifier;
                break;
            }

            if (!ScriptableStatModifier)
            {
                DisplayModifiers(ScriptableEquipment._statModifierTable);
                return false;
            }

            ModifierID = ScriptableStatModifier._modifierID;
        }

        ItemData._damageTypeOverride = DamageType;
        ItemData._useDamageTypeOverride = UseDamageTypeOverride;
        ItemData._modifierID = ModifierID;

        Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._purchaseItemSound);

        return false;
    }

    private void DisplayDamageTypes()
    {
        Type Type = typeof(DamageType);
        List<string> DamageTypes = [];

        byte Value = 0;
        foreach (string Name in Enum.GetNames(Type))
        {
            DamageTypes.Add(Main.Instance.Translate("Commands.Enchant.DamageTypes.DamageType", Value, Name));
            Value++;
        }

        ChatBehaviour._current.New_ChatMessage(
            Main.Instance.Translate(
                "Commands.Enchant.DamageTypes",
                string.Join(
                    Main.Instance.Translate("Commands.Enchant.DamageTypes.Separator"),
                    DamageTypes
                )
            )
        );
    }

    private void DisplayModifiers(ScriptableStatModifierTable ScriptableStatModifierTable)
    {
        List<string> Modifiers = [];

        foreach (StatModifierSlot StatModifierSlot in ScriptableStatModifierTable._statModifierSlots)
            Modifiers.Add(
                Main.Instance.Translate(
                    "Commands.Enchant.Modifiers.Modifier",
                    StatModifierSlot._equipModifier._modifierID,
                    StatModifierSlot._equipModifier._modifierTag
                )
            );

        ChatBehaviour._current.New_ChatMessage(
            Main.Instance.Translate(
                "Commands.Enchant.Modifiers",
                string.Join(
                    Main.Instance.Translate("Commands.Enchant.Modifiers.Separator"),
                    Modifiers
                )
            )
        );
    }
}