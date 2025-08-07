using Tanuki.Atlyss.API.Commands;

namespace Tanuki.Atlyss.FluffUtilities.Commands;

internal class Item : ICommand
{
    public void Execute(string[] Arguments)
    {
        if (Arguments.Length == 0 || Arguments.Length > 2)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Item.InvalidParameters"));
            return;
        }

        bool Given = false;
        int Quantity = 1;
        if (Arguments.Length == 2)
        {
            if (!int.TryParse(Arguments[1], out Quantity))
            {
                ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Item.UnparseableQuantity"));
                return;
            }

            if (Quantity < 1)
                Quantity = 1;
        }

        ScriptableItem ScriptableItem = GameManager._current.Locate_Item(Arguments[0]);

        if (ScriptableItem is null)
        {
            ChatBehaviour._current.New_ChatMessage(Main.Instance.Translate("Commands.Item.ItemNotFound", Arguments[0]));
            return;
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
    }
}