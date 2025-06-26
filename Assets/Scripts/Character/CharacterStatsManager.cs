using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Stamina Regeneration")]
    [SerializeField] float staminaRegenerationAmount = 2f;
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] float staminaRegenerationDelay = 2f;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public int CalculateStaminaBasedOnEnduranceLevel(float endurance)
    {
        float stamina = 0f;

        // Create an EQUATION for how we want our stamina to be calculated

        stamina = endurance * 10;

        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerationStamina()
    {
        if (!character.IsOwner)
        {
            return;
        }

        if (character.characterNetworkManager.isSprinting.Value)
        {
            return;
        }

        if (character.isPerformingAction)
        {
            return;
        }

        staminaRegenerationTimer += Time.deltaTime;

        if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
        {
            staminaTickTimer += Time.deltaTime;

            if (staminaTickTimer >= 0.1)
            {
                staminaTickTimer = 0;
                character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
            }
        }
    }

    public virtual void ResetStaminaRegenerationTimer(float previousStaminaAmount, float currentStaminaAmount)
    {
        // Only reset the regeneration stamina timer if the action used stamina
        if (currentStaminaAmount < previousStaminaAmount)
        {
            staminaRegenerationTimer = 0;
        }
    }
}
