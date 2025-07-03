using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item
{
    // Animator Controller Override (Change Attack Animations based on weapon you are currently using)

    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Requirements")]
    public int strengthREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;
    public int faithREQ = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int holyDamage = 0;
    public int lightningDamage = 0;

    // Weapon guard ABSORPTIONS (Blocking Power)

    [Header("Weapon Poise")]
    public float poiseDamage = 10;
    // Offensive poise bonus when attacking

    // Weapon Modifiers
    // Light Attack Modifier
    // Heavy Attack Modifier
    // Critical Damage Modifier ect

    [Header("Stamina Costs")]
    public int staminaBaseCost = 20;
    // Running attack stamina cost modifier
    // Light attack stamina cost modifier
    // Heavy attack stamina cost modifier ect

    // Item based actions (RB, RT, LB, LT)

    // Ash of War

    // Blocking Sounds
}
