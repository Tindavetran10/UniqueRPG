using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float walkingSpeed = 1;
    [SerializeField] float runningSpeed = 3.5f;
    [SerializeField] float sprintingSpeed = 6.5f;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] int sprintingStaminaCost = 2;

    [Header("Dodge")]
    private Vector3 rollDirection;
    [SerializeField] float dodgeStaminaCost = 25;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;       
        }
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            // If not lock on, pass the move amount
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
    }

    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }

    private void HandleGroundedMovement()
    {
        if (!player.canMove)
        {
            return;
        }
        GetMovementValues();

        // Our move is based on our cameras 
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
        }
    }

    private void HandleRotation()
    {
        if (!player.canRotate)
        {
            return;
        }
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            // Set sprinting to FALSE
            player.playerNetworkManager.isSprinting.Value = false;
        }

        // If player is out of STAMINA, Set sprinting to FALSE
        if (player.playerNetworkManager.currentStamina.Value <= 0)
        {
            player.playerNetworkManager.isSprinting.Value = false;
            return;
        }

        // If player is moving, Set sprinting to TRUE
        if (moveAmount >= 0.5f)
        {
            player.playerNetworkManager.isSprinting.Value = true;
        }
        // If player is Stationary, Set Sprinting to FALSE
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
        }
    }

    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction)
        {
            return;
        }

        if (player.playerNetworkManager.currentStamina.Value <= 0)
            return;

        // If we are moving when we attempt to Dodge, we perform a dodge
        if (PlayerInputManager.instance.moveAmount > 0)
        {
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            rollDirection.y = 0;
            rollDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            // Perform a roll animation
            player.playerAnimatorManager.PlayerTargetActionAnimation("Roll_Forward_01", true, true);
        }
        // If we are stationary, we perform a backstep
        else
        {
            // Perform a backstep animation
            player.playerAnimatorManager.PlayerTargetActionAnimation("Back_Step_01", true, true);
        }

        player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
    }
}
