using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game")]
    [SerializeField] int requiredDeliveries = 3;

    [Header("UI")]
    [SerializeField] GameObject victoryScreen;

    [Header("Scene")]
    [SerializeField] string menuSceneName = "SCN_MainMenu";

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

        // CAMBIAR A UI MAP
        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("UI");
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

        // 🔥 DESACTIVAR TODOS LOS ENEMIGOS
        EnemyIA[] enemies =
            FindObjectsByType<EnemyIA>(
                FindObjectsSortMode.None
            );

        foreach (EnemyIA enemy in enemies)
        {
            enemy.DisableEnemy();
        }

        // 🔥 VOLVER AL MENU EN 10s
        StartCoroutine(ReturnToMenuRoutine());
    }

    // 🔥 LLAMAR DESDE PLAYERHEALTH AL MORIR
    public void Defeat()
    {
        // 🔥 DESACTIVAR TODOS LOS ENEMIGOS
        EnemyIA[] enemies =
            FindObjectsByType<EnemyIA>(
                FindObjectsSortMode.None
            );

        foreach (EnemyIA enemy in enemies)
        {
            enemy.DisableEnemy();
        }

        StartCoroutine(ReturnToMenuRoutine());
    }

    IEnumerator ReturnToMenuRoutine()
    {
        yield return new WaitForSeconds(10f);

        SceneManager.LoadScene(menuSceneName);
    }
}