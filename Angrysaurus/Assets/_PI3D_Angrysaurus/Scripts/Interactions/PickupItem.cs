using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemID;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Pickup(Transform carryPoint)
    {
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.SetParent(carryPoint);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}