using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Player/Stats")]
public class Stats : ScriptableObject
{
    private Dictionary<StatType, int> statList;

    public enum StatType
    {
        Default,
        Level, 
        Strength,
        Agility,
        MaxHP,
        MaxMP,
        CurrentHP,
        CurrentMP,
        Attack,
        Defense,
        EXP
    }

    public Stats()
    {
        statList = new Dictionary<StatType, int>
        {
            { StatType.Level, 1 },
            { StatType.Strength, 4 },
            { StatType.Agility, 4 },
            { StatType.MaxHP, 15 },
            { StatType.MaxMP, 0 },
            { StatType.CurrentHP, 15 },
            { StatType.CurrentMP, 0 },
            { StatType.Attack, 4 },
            { StatType.Defense, 4 },
            { StatType.EXP, 0 }
        };
    }

    public Dictionary<StatType, int> GetStatList()
    {
        return statList;
    }

    public void ChangeStat(StatType type, int value)
    {
        statList[type] = value;
    }
}
