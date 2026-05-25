using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform carryPoint;

    PickupItem nearbyPickup;
    PickupItem carriedItem;

    public bool IsCarrying => carriedItem != null;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (!IsCarrying && nearbyPickup != null)
        {
            Pickup(nearbyPickup);
        }
    }

    void Pickup(PickupItem item)
    {
        carriedItem = item;

        item.Pickup(carryPoint);

        Debug.Log(
            "Objeto recogido: " +
            item.name
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        PickupItem pickup =
            other.GetComponent<PickupItem>();

        if (pickup != null)
        {
            nearbyPickup = pickup;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickupItem pickup =
            other.GetComponent<PickupItem>();

        if (
            pickup != null &&
            pickup == nearbyPickup
        )
        {
            nearbyPickup = null;
        }
    }
}