using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    public PlayerManager player;
    PlayerControls playerControls;

    [Header("Player Movement Input")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("Camera Input")]
    [SerializeField] Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // When scene changes, run this logic
        SceneManager.activeSceneChanged += OnSceneChanged;

        instance.enabled = false;
    }

    private void Update()
    {
        HandlePlayerMovementInput();

        HandleCameraMovementInput();
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        // If the game loading into our world scene, enable our player controls
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        else
        {
            instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        // If we destroy this object, UNSUBSCRIBE from this event
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }


    // If we minimize or lower the window, stop adjusting inputs
    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    public void HandlePlayerMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
        if (moveAmount <= 0.5f && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }else if (moveAmount > 0.5f && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        if (player == null)
        {
            return;
        }

        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }
}
