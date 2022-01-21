using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Stats stats;

    public event EventHandler OnStatChange;

    private Dictionary<Equipment.Type, Equipment> currentEquipment;

    private void Awake()
    {
        //load it from file? make it SO?
        currentEquipment = new Dictionary<Equipment.Type, Equipment>();
    }

    private void Start()
    {

    }

    public void AddItem(Item item)
    {
        inventory.AddItem(item);
    }

    public void EquipItem(Equipment equipment)
    {
        currentEquipment[equipment.equipmentType] = equipment;
        //stats.GetStatList()[equipment.Stat.Key] = equipment.Stat.Value;
    }

    //---------------------player stats-----------------------------------

    public Dictionary<Stats.StatType, int> GetStatList()
    {
        return stats.GetStatList();
    }

    public void TakeDamage(int damage)
    {
        
    }
    
}
