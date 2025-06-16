using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{
    public CharacterController characterController;

    CharacterNetworkManager characterNetworkManager;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);

        characterController = GetComponent<CharacterController>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
    }

    protected virtual void Update()
    {
        // If this character is being controlled from our side, then assign its network position to the position of our transform
        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        // if this character is being controlled from else where, then assign its position here locally by the position of its network transform
        else
        {
            transform.position = Vector3.SmoothDamp
                                (transform.position, 
                                characterNetworkManager.networkPosition.Value, 
                                ref characterNetworkManager.networkPositionVelocity, 
                                characterNetworkManager.networkPositionSmoothTime);

            transform.rotation = Quaternion.Slerp
                                (transform.rotation, 
                                characterNetworkManager.networkRotation.Value, 
                                characterNetworkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate()
    {

    }
}
