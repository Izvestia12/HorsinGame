using UnityEngine;

public class SimplePickUp : MonoBehaviour
{
    [Header("Settings")]
    public float pickupRange = 4.0f;     // ระยะการหยิบ
    public float pickupRadius = 0.3f;    // ความกว้างของลำแสง (ช่วยให้หยิบง่ายขึ้น)
    public Transform holdParent;
    public GameObject pickUpPrompt;

    private GameObject heldObj;

    void Update()
    {
        // Debug: วาดเส้นให้เห็นทิศทางการเล็งในหน้า Scene
        Debug.DrawRay(transform.position, transform.forward * pickupRange, Color.green);

        if (heldObj == null)
        {
            CheckForPickable();
        }
        else
        {
            if (pickUpPrompt != null) pickUpPrompt.SetActive(false);
            MoveHeldObject();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null) PickupObject();
            else DropObject();
        }
    }

    void CheckForPickable()
    {
        RaycastHit hit;
        // ใช้ SphereCast เพื่อให้มีรัศมีการตรวจจับที่กว้างขึ้น
        if (Physics.SphereCast(transform.position, pickupRadius, transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("Pickable"))
            {
                if (pickUpPrompt != null) pickUpPrompt.SetActive(true);
                return;
            }
        }
        if (pickUpPrompt != null) pickUpPrompt.SetActive(false);
    }

    void PickupObject()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, pickupRadius, transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("Pickable"))
            {
                heldObj = hit.collider.gameObject;
                Rigidbody rb = heldObj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;
                    rb.isKinematic = true;
                }
                heldObj.transform.SetParent(holdParent);
                // บังคับให้ของวาร์ปมาที่จุดถือทันที
                heldObj.transform.localPosition = Vector3.zero;
            }
        }
    }

    void MoveHeldObject()
    {
        // ใช้ Lerp เพื่อให้ของเคลื่อนที่ตามมืออย่างนุ่มนวลและไม่สั่น
        heldObj.transform.position = Vector3.Lerp(heldObj.transform.position, holdParent.position, Time.deltaTime * 20f);
        heldObj.transform.rotation = Quaternion.Lerp(heldObj.transform.rotation, holdParent.rotation, Time.deltaTime * 20f);
    }

    void DropObject()
    {
        if (heldObj == null) return;
        Rigidbody rb = heldObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        heldObj.transform.SetParent(null);
        heldObj = null;
    }
}