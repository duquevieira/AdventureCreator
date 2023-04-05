using System.Collections.Generic;

namespace MoreMountains.InventoryEngine
{
    public class FakeInventory : Inventory
    {
        public List<ItemGroup> Items;

        public FakeInventory()
        {
            Items = new List<ItemGroup>();
        }

        public override int GetQuantity(string itemName)
        {
            int amount = 0;
            foreach (ItemGroup item in Items)
            {
                if(item.getItemName().Equals(itemName))
                {
                    amount += item.getItemAmount();
                }
            }
            return amount;
        }

        public override bool RemoveItemByID(string itemName, int itemAmount)
        {
            int amount = itemAmount;
            bool foundItem = false;
            List<ItemGroup> listToRemove = new List<ItemGroup>();
            foreach (ItemGroup item in Items)
            {
                if (item.getItemName().Equals(itemName))
                {
                    foundItem = true;
                    if(item.getItemAmount() >= amount)
                    {
                        item.removeItemAmount(amount);
                        break;
                    }
                    else
                    {
                        amount -= item.getItemAmount();
                        item.removeItemAmount(item.getItemAmount());
                    }
                    if(item.getItemAmount() == 0)
                    {
                        listToRemove.Add(item);
                    }
                }
            }
            foreach (ItemGroup toRemove in listToRemove)
            {
                Items.Remove(toRemove);
            }
            return foundItem;
        }
    }
}
