using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorMovementParameters(float horiozntalValue, float verticalValue, bool isSprinting)
    {
        if (isSprinting)
        {
            verticalValue = 2;
        }

        character.animator.SetFloat(horizontal, horiozntalValue, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, verticalValue, 0.1f, Time.deltaTime);
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
