using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    float vertical;
    float horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameters(float horiozntalValue, float verticalValue)
    {
        character.animator.SetFloat("Horizontal", horiozntalValue, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
    }

    public virtual void PlayerTargetActionAnimation(string targetAnimation, 
                                                    bool isPerformingAction, 
                                                    bool applyRootMotion = true,
                                                    bool canRotate = false,
                                                    bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        // Can be used to stop character from attempting new action
        // THIS FLAG WILL TURN TRUE IF YOU ARE STUNNED
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

        // Tell the Server/Host we played an animation, and to play that animation for everybody else present
        character.characterNetworkManager.NotifyTheServerofActionAnimationServerRPC(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }
}
