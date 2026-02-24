using UnityEngine;
using UnityEngine.UI;

public class PushQTE : MonoBehaviour
{
    public int requiredPresses = 15;
    public float timeLimit = 5f;
    public Slider qteBar;

    private int currentPresses = 0;
    private float timer;
    private bool isActive = false;
    private WeepingAngel angelScript; // แก้ไขชื่อคลาสที่นี่

    public void Begin(WeepingAngel script) // แก้ไขพารามิเตอร์ที่นี่
    {
        isActive = true;
        angelScript = script;
        currentPresses = 0;
        timer = timeLimit;
        if (qteBar != null) qteBar.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;
        if (qteBar != null) qteBar.value = (float)currentPresses / requiredPresses;

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentPresses++;
        }

        if (currentPresses >= requiredPresses)
        {
            Finish(true);
        }
        else if (timer <= 0)
        {
            Finish(false);
        }
    }

    void Finish(bool success)
    {
        isActive = false;
        if (qteBar != null) qteBar.gameObject.SetActive(false);

        if (success)
        {
            angelScript.OnFallDown();
        }
        else
        {
            // ถ้าล้มเหลว ให้รูปปั้นกลับมาขยับได้ใหม่ หรือสั่งให้ผู้เล่นตาย
            Debug.Log("QTE Failed!");
            // angelScript (เพิ่มคำสั่งลงโทษที่นี่ได้ครับ)
        }
    }
}