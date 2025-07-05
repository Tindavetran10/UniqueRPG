using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Characters")]
    public CharacterManager characterCausingDamage; // When calculate damage this is use to checks for attackers damage modifiers, effects ect.
}
