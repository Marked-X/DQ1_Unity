using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Player/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField]
    private List<Item> itemList;
    public event EventHandler OnItemListChanged;

    public Inventory()
    {
        itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        itemList.Remove(item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public List<Equipment> GetEquipmentList()
    {
        List<Equipment> returnable = new List<Equipment>();
        foreach(Item item in itemList)
        {
            if(item.GetType() == typeof(Equipment))
            {
                returnable.Add((Equipment)item);
            }
        }
        return returnable;
    }
}
