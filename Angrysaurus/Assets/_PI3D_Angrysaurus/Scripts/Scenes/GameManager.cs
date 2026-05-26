using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game")]
    [SerializeField] int requiredDeliveries = 3;

    [Header("UI")]
    [SerializeField] GameObject victoryScreen;

    int currentDeliveries;

    PlayerInput playerInput;

    void Awake()
    {
        Instance = this;

        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
        }

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerInput =
                player.GetComponent<PlayerInput>();
        }
    }

    void Update()
    {
        // DEBUG CLICK
        if (
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame
        )
        {
            Debug.Log("CLICK DETECTADO");
        }
    }

    public void AddDelivery()
    {
        currentDeliveries++;

        Debug.Log(
            "Deliveries: " +
            currentDeliveries +
            "/" +
            requiredDeliveries
        );

        if (currentDeliveries >= requiredDeliveries)
        {
            Victory();
        }
    }

    void Victory()
    {
        Debug.Log("VICTORY");

        // UI
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
        }

        // CURSOR
        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        // DESACTIVAR INPUT
        if (playerInput != null)
        {
            playerInput.DeactivateInput();
        }

        // DESACTIVAR MOVIMIENTO
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerMovement movement =
                player.GetComponent<PlayerMovement>();

            if (movement != null)
            {
                movement.enabled = false;
            }

            PlayerShoot shoot =
                player.GetComponent<PlayerShoot>();

            if (shoot != null)
            {
                shoot.enabled = false;
            }
        }

        // DESACTIVAR CAMERA
        if (Camera.main != null)
        {
            CameraFollow cam =
                Camera.main.GetComponent<CameraFollow>();

            if (cam != null)
            {
                cam.DisableCameraControl();
            }
        }
    }
}