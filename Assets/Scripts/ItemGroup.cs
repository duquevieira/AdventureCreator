using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGroup
{
    private string _itemName;
    private int _amount;

    public ItemGroup(string itemName, int amount)
    {
        _itemName = itemName;
        _amount = amount;
    }

    public string getItemName()
    {
        return _itemName;
    }

    public int getItemAmount()
    {
        return _amount;
    }

    public void addItemAmount(int amount)
    {
        _amount = _amount + amount;
    }

    public void removeItemAmount(int amount)
    {
        _amount = _amount - amount;
    }
}
