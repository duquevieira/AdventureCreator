using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ItemGroup
{
    [SerializeField]
    private string _itemName;
    [SerializeField]
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
        _amount += amount;
    }

    public void removeItemAmount(int amount)
    {
        _amount -= amount;
    }
}
