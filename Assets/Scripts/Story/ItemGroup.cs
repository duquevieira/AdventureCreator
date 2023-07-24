using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ItemGroup
{
    public string ItemName;
    public int Amount;

    public ItemGroup(string itemName, int amount)
    {
        ItemName = itemName;
        Amount = amount;
    }

    public string getItemName()
    {
        return ItemName;
    }

    public int getItemAmount()
    {
        return Amount;
    }

    public void addItemAmount(int amount)
    {
        Amount += amount;
    }

    public void removeItemAmount(int amount)
    {
        Amount -= amount;
    }
}
