using System.Collections.Generic;
using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class Item : ICommand
{
    public bool Execute(string[] Arguments)
    {
        if (Arguments.Length == 0)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Item.InvalidParameters"));
            return false;
        }

        int Quantity = 1;
        int ItemNameParts = Arguments.Length;
        if (Arguments.Length > 1)
        {
            if (ushort.TryParse(Arguments[Arguments.Length - 1], out ushort ParsedQuantity))
            {
                if (ParsedQuantity > 0)
                {
                    Quantity = ParsedQuantity;
                    ItemNameParts -= 1;
                }
            }
        }

        string ItemName = string.Join(" ", Arguments, 0, ItemNameParts);
        bool Given = false;

        ScriptableItem ScriptableItem = GameManager._current.Locate_Item(ItemName);

        if (!ScriptableItem)
        {
            foreach (KeyValuePair<string, ScriptableItem> CachedScriptableItem in Game.Accessors.GameManager._cachedScriptableItems(GameManager._current))
            {
                if (CachedScriptableItem.Key.IndexOf(ItemName, System.StringComparison.InvariantCultureIgnoreCase) < 0)
                    continue;

                ScriptableItem = CachedScriptableItem.Value;
                break;
            }
        }

        if (!ScriptableItem)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Item.ItemNotFound", ItemName));
            return false;
        }

        while (Quantity > 0)
        {
            int Current = Quantity > ScriptableItem._maxStackAmount ? ScriptableItem._maxStackAmount : Quantity;

            if (Player._mainPlayer._pInventory.Check_InventoryFull(ScriptableItem, Current))
            {
                Current = ScriptableItem._maxStackAmount - Player._mainPlayer._pInventory.Check_ItemQuantity(ScriptableItem);
                if (Current < 1)
                    break;

                Quantity = 0;
            }

            ItemData itemData = new()
            {
                _itemName = ScriptableItem._itemName,
                _quantity = Current,
                _maxQuantity = ScriptableItem._maxStackAmount,
                _isEquipped = false,
                _isAltWeapon = false,
                _slotNumber = 0
            };

            Player._mainPlayer._pInventory.Add_Item(itemData, true);
            Quantity -= Current;
            Given = true;
        }

        if (Given)
            Player._mainPlayer._pSound._aSrcGeneral.PlayOneShot(Player._mainPlayer._pSound._purchaseItemSound);

        return false;
    }
}