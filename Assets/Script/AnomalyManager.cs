using UnityEngine;
using System.Collections.Generic;

public class AnomalyManager : MonoBehaviour
{
    [Header("Anomaly List")]
    public List<AnomalyObject> allAnomalies;
    public int anomaliesToWin = 1; // จำนวนที่ต้องแก้เพื่อผ่านด่าน
    private int currentFixedCount = 0;
    private bool anomalyActive = false;

    public void SpawnOneAnomaly()
    {
        // สร้าง List ใหม่ชั่วคราวเพื่อเก็บเฉพาะ Anomaly ที่ยังไม่โดน Destroy
        List<AnomalyObject> validAnomalies = new List<AnomalyObject>();

        foreach (var a in allAnomalies)
        {
            if (a != null) // ตรวจสอบว่าวัตถุยังไม่ถูก Destroy
            {
                validAnomalies.Add(a);
            }
        }

        if (validAnomalies.Count > 0 && !anomalyActive)
        {
            int randomIndex = Random.Range(0, validAnomalies.Count);
            validAnomalies[randomIndex].TriggerAnomaly(); // เรียกใช้เฉพาะตัวที่มีอยู่จริง
            anomalyActive = true;
            Debug.Log("Anomaly เกิดแล้ว");
        }
        else
        {
            Debug.Log("ไม่มี Anomaly เหลือให้สุ่มแล้ว หรือมีอันเก่าค้างอยู่");
        }
    }

    public void ClearCurrentAnomaly()
    {
        foreach (var anomaly in allAnomalies)
        {
            if (anomaly == null) continue;
            anomaly.ShowNormal();
        }
        anomalyActive = false;
        Debug.Log("ลบ Anomaly เดิมเรียบร้อย กลับเป็นปกติ");
    }

    // ฟังก์ชันรับรายงานการแก้ไข (สำคัญ: ต้องกด Save ไฟล์นี้ Error ถึงจะหาย)
    public void ReportFixedAnomaly()
    {
        currentFixedCount++;
        Debug.Log("ได้รับรายงานการแก้ไข Anomaly แล้ว! แก้ไปแล้ว: " + currentFixedCount);

        if (currentFixedCount >= anomaliesToWin)
        {
            Debug.Log("ยินดีด้วย! คุณแก้ไข Anomaly ครบตามกำหนดแล้ว");
            // ใส่คำสั่งผ่านด่านตรงนี้ เช่น SceneManager.LoadScene(...)
        }
    }
}