using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera mainCamera;

    [Header("Interaction")]
    [SerializeField] float interactionDistance = 4f;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Ray ray = mainCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance
        ))
        {
            IInteractable interactable =
                hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}