using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment")]
public class Equipment : Item
{
    [SerializeField]
    private Stats.StatType statType;
    [SerializeField]
    private int statValue;
    [SerializeField]
    public Type equipmentType;

    public enum Type
    {
        Default,
        Weapon,
        Shield,
        Armor,
        Accessory
    }

    public KeyValuePair<Stats.StatType, int> Stat { 
        get 
        {
            return new KeyValuePair<Stats.StatType, int>(statType, statValue);
        } 
    }

    public override void Activate()
    {
        //equip
        Debug.Log("Equiped " + this.Name);
    }
}
