using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    // Process Instant Effects (Take Damage, Heal)
    #region Instant Effects

    public void ProcessInstantEffects(InstantCharacterEffect effect)
    {
        // Take in an effect
        effect.ProcessEffect(character);
        // Process it
    }

    #endregion

    // Process Timed Effects (Poison, Build Ups)
    #region Timed Effects

    #endregion

    // Process Static Effects (Adding/Removing buffs from TALISMANS ect)
    #region Static Effects

    #endregion
}
