using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    public string name;

    public enum Type
    {
        DUMMY,
        MAGE,
        WARRIOR
    }

    public Type enemyType;

    public int maxHealth;
    public int curHealth;

    public int maxMana;
    public int curMana;

    public int baseDamage;
    public int varDamage;

    public int defense;

    public int block;
}
