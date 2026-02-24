using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("UI & References")]
    public TextMeshProUGUI timeText;
    public AnomalyManager anomalyManager;
    public Transform spawnPoint;
    public GameObject player;

    [Header("Settings")]
    public float realTimeForOneHour = 300f; // 5 นาที = 1 ชม. ในเกม

    private float timer = 0f;
    private int currentHour = 19;
    private bool isAnomalyTriggered = false;

    void Start()
    {
        ResetToStart();
    }

    void Update()
    {
        if (!isAnomalyTriggered)
        {
            timer += Time.deltaTime;
            float progress = timer / realTimeForOneHour;
            int minutes = Mathf.FloorToInt(progress * 60);

            if (timeText != null)
                timeText.text = string.Format("{0}:{1:00}", currentHour, minutes);

            if (timer >= realTimeForOneHour)
            {
                TriggerAnomalyPhase();
            }
        }
    }

    public void TriggerAnomalyPhase()
    {
        isAnomalyTriggered = true;
        currentHour++;
        if (timeText != null) timeText.text = string.Format("{0}:00", currentHour);
        if (anomalyManager != null) anomalyManager.SpawnOneAnomaly();
    }

    public void SuccessNextHour()
    {
        timer = 0f;
        isAnomalyTriggered = false;
    }

    public void SkipToNextAnomaly()
    {
        timer = realTimeForOneHour;
        if (!isAnomalyTriggered) TriggerAnomalyPhase();
    }

    public void ResetToStart()
    {
        currentHour = 19;
        timer = 0f;
        isAnomalyTriggered = false;
        if (timeText != null) timeText.text = "19:00";
        WarpPlayer();
    }

    public void WarpPlayer()
    {
        if (spawnPoint != null && player != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
            if (cc != null) cc.enabled = true;
        }
    }

    public int GetCurrentHour() => currentHour;
    public bool IsInAnomalyTime() => isAnomalyTriggered;
}