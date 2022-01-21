using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : ScriptableObject
{
    public Sprite Sprite;
    public int MaxHP;
    public int CurrentHP;
    public int Attack;
    public int AttackSpeed;
    public int Defense;
    public int Experience;
    public int Gold;
    public int Evasion;
    public int FizzleResistance;
    public int SnoozeResistance;
    public int FireResistance;

    public abstract void TakeTurn();
    public void ReadyUp()
    {
        CurrentHP = MaxHP;
    }
}
