using System;
using System.Collections.Generic;
using Tanuki.Atlyss.API.Core.Commands;
using Tanuki.Atlyss.Core.Extensions;
using Tanuki.Atlyss.FluffUtilities.Helpers;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

[CommandMetadata(EExecutionSide.Client, typeof(Core.Policies.Commands.Caller.Player))]
internal sealed class Item : ICommand
{
    private IReadOnlyDictionary<string, ScriptableItem> CachedScriptableItems => Game.Accessors.GameManager._cachedScriptableItems(GameManager._current);

    public void Execute(IContext context)
    {
        IReadOnlyList<string> arguments = context.Arguments;
        Player player = Player._mainPlayer;

        if (arguments.Count == 0)
        {
            Chat.AddTranslatedMessage("Commands.Item.InvalidParameters");
            return;
        }

        int quantity = 1;
        int namePartsCount = arguments.Count;

        if (namePartsCount > 1 && ushort.TryParse(arguments[namePartsCount - 1], out ushort parsedQuantity) && parsedQuantity > 0)
        {
            quantity = parsedQuantity;
            namePartsCount -= 1;
        }

        string itemName = Core.Extensions.String.Join(" ", arguments, 0, namePartsCount);

        if (!CachedScriptableItems.TryGetValueFlexible(itemName, out ScriptableItem scriptableItem, false, StringComparison.OrdinalIgnoreCase))
        {
            Chat.AddTranslatedMessage("Commands.Item.ItemNotFound", itemName);
            return;
        }

        PlayerInventory playerInventory = player._pInventory;
        bool given = false;

        while (quantity > 0)
        {
            int current = quantity > scriptableItem._maxStackAmount ? scriptableItem._maxStackAmount : quantity;

            if (playerInventory.Check_InventoryFull(scriptableItem, current))
            {
                current = scriptableItem._maxStackAmount - playerInventory.Check_ItemQuantity(scriptableItem);
                if (current < 1)
                    break;

                quantity = 0;
            }

            ItemData itemData = new()
            {
                _itemName = scriptableItem._itemName,
                _quantity = current,
                _maxQuantity = scriptableItem._maxStackAmount,
                _isEquipped = false,
                _isAltWeapon = false,
                _slotNumber = 0
            };

            playerInventory.Add_Item(itemData, true);
            quantity -= current;
            given = true;
        }

        if (given)
            player._pSound._aSrcGeneral.PlayOneShot(player._pSound._purchaseItemSound);
    }
}