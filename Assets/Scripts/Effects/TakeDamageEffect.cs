using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage; // If the damage is caused by another characters attack it will be stored here

    [Header("Damage")]
    public float physicalDamage = 0; // Will be split into "Standard", "Strike", "Slash" and "Pierce"
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float holyDamage = 0;

    [Header("Final Damage")]
    public int finalDamageDealt = 0; // The damage the character takes after ALL calculations have been made

    // (To Do) Build Ups
    // Build up effect amount

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundFX; // Used on top of regular SFX if there is elemental damage present (Magic/Fire/Lightning/Holy)

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;              // Used to determine what damage animation to play (Move backwards, to the left, to the right ect) 
    public Vector3 contactPoint;            // Used to determine where the blood FX Instantiate

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        if (character.isDead.Value)
        {
            return;
        }

        // Check for "INVULNERABILITY"

        // Calculate damage
        CalculateDamage(character);
        // Check which direction, damage came from
        // Play a damage animation
        // Check for build ups (Poison, Bleed ect)
        // Play Damage Sound FX
        // Play damage VFX (Blood)

        // If character is A.I, Check for new target if character causing damage is present
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (!character.IsOwner) { return; }
        if (characterCausingDamage != null)
        {
            // Check for damage modifiers and modify base damage (Physical/ elemental damage buff)
        }

        // Check character for flat defenses and subtract them from the damage

        // Check character for armor absorptions, and subtract the percentage of the damage

        // Add all damage types together, and apply final damage
        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }

        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

        // Calculate Poison damage to determine if the character will be STUNNED
    }
}
