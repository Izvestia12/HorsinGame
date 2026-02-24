using UnityEngine;

public class PhotoSystem : MonoBehaviour
{
    public bool isAnomalyCaptured = false;

    public bool CheckForAnomaly()
    {
        RaycastHit hit;
        // ปรับระยะเป็น 50 เมตรเพื่อให้ส่องถึงวัตถุได้ง่ายขึ้น
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            Debug.Log("กล้องส่องโดนวัตถุชื่อ: " + hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Anomaly")) return true;
        }
        return false;
    }
}