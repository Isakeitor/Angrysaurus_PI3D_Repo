using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject dialoguePanel;

    bool isOpen;

    void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    public void ToggleDialogue()
    {
        isOpen = !isOpen;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(isOpen);
        }

        // CURSOR
        Cursor.lockState =
            isOpen
            ? CursorLockMode.None
            : CursorLockMode.Locked;

        Cursor.visible = isOpen;

        // PLAYER
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerMovement movement =
                player.GetComponent<PlayerMovement>();

            if (movement != null)
            {
                movement.enabled = !isOpen;
            }

            PlayerShoot shoot =
                player.GetComponent<PlayerShoot>();

            if (shoot != null)
            {
                shoot.enabled = !isOpen;
            }
        }

        // CAMERA
        if (Camera.main != null)
        {
            CameraFollow cam =
                Camera.main.GetComponent<CameraFollow>();

            if (cam != null)
            {
                if (isOpen)
                    cam.DisableCameraControl();
                else
                    cam.EnableCameraControl();
            }
        }
    }
}