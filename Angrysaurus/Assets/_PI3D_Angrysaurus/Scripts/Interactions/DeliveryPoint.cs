using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] string requiredItemID;

    [Header("VFX")]
    [SerializeField] GameObject deliveryVFX;

    bool completed;

    private void OnTriggerEnter(Collider other)
    {
        if (completed)
            return;

        // BUSCAR PICKUP
        PickupItem item =
            other.GetComponent<PickupItem>();

        if (item == null)
            return;

        // COMPROBAR ID
        if (item.itemID != requiredItemID)
        {
            Debug.Log("Objeto incorrecto");

            return;
        }

        completed = true;

        Debug.Log("Entrega correcta");

        // VFX
        if (deliveryVFX != null)
        {
            Instantiate(
                deliveryVFX,
                item.transform.position,
                Quaternion.identity
            );
        }

        // DESTRUIR OBJETO
        Destroy(item.gameObject);

        // SUMAR ENTREGA
        GameManager.Instance.AddDelivery();
    }
}