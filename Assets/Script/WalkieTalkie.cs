using UnityEngine;

public class WalkieTalkie : MonoBehaviour
{
    public PhoneAppManager phoneManager;
    public PhotoSystem photoSystem;
    public TimeManager timeManager;
    public KeyCode reportKey = KeyCode.R;

    void Update()
    {
        if (Input.GetKeyDown(reportKey))
        {
            FinalReport();
        }
    }

    void FinalReport()
    {
        if (timeManager == null) return;

        bool isAnomalyActive = timeManager.IsInAnomalyTime();

        if (!isAnomalyActive)
        {
            Debug.Log("ข้ามไปช่วง Anomaly");
            timeManager.SkipToNextAnomaly();
            return;
        }

      
        if (phoneManager.DidUserSendPhoto() && photoSystem.isAnomalyCaptured)
        {
            Debug.Log("รายงานถูก! ไปชั่วโมงถัดไป");
            phoneManager.ClearAllPhotos();
            if (FindObjectOfType<AnomalyManager>() != null)
                FindObjectOfType<AnomalyManager>().ClearCurrentAnomaly();

            timeManager.SuccessNextHour(); // เริ่มนับเวลาชั่วโมงใหม่
            timeManager.SkipToNextAnomaly(); // แสดงชั่วโมงใหม่ทันที
        }
        else
        {
            // รายงานผิด หรือ ข้อมูลไม่ครบ
            Debug.Log("รายงานผิด! กลับไป 19:00");
            timeManager.ResetToStart();
            phoneManager.ClearAllPhotos();
        }
    }
}