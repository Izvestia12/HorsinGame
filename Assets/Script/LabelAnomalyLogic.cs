using UnityEngine;

public class LabelAnomalyLogic : MonoBehaviour
{
    public GameObject floatingLabel;
    public GameObject snapPoint;
    public GameObject fixedLabel;

    public void OnAnomalyTriggered()
    {
        fixedLabel.SetActive(false);
        floatingLabel.SetActive(true);
        snapPoint.SetActive(true);
        this.tag = "Anomaly"; // ถ่ายรูปตอนป้ายหาย
    }

    public void MarkAsFixed()
    {
        // เปลี่ยนเป็น Tag Fixed เพื่อให้กล้อง PhoneAppManager ตรวจเจอและส่งผ่านด่านได้
        this.tag = "Fixed";

        AnomalyManager manager = FindObjectOfType<AnomalyManager>();
        if (manager != null)
        {
            manager.ReportFixedAnomaly();
            Debug.Log("ส่งรายงานสำเร็จ: ป้ายถูกแก้ไขแล้ว");
        }
        else
        {
            Debug.LogError("ไม่พบ AnomalyManager ในฉาก!");
        }
    }
}