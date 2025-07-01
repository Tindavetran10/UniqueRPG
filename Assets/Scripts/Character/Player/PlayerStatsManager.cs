using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    PlayerManager player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();

        // When we make a character creation menu, and set the stats depending on the class, this will be calculated there
        // Until then however, Stats are never calculated, so we do it here on start, if a save file exists they will be over written when loading into scene
        CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
        CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
    }
}
