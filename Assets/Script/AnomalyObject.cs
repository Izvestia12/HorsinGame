using UnityEngine;

public class AnomalyObject : MonoBehaviour
{
    [Header("Settings")]
    public string anomalyName;      
    public GameObject normalState;  
    public GameObject anomalyState; 

    private bool _isAnomalyActive = false;

    void Start()
    {
        ShowNormal();
    }

    public void TriggerAnomaly()
    {
        if (normalState != null) normalState.SetActive(false);
        if (anomalyState != null) anomalyState.SetActive(true);

        LabelAnomalyLogic labelLogic = GetComponent<LabelAnomalyLogic>();
        if (labelLogic != null)
        {
            labelLogic.OnAnomalyTriggered();
        }
    }

    public void ShowNormal()
    {
        if (normalState != null) normalState.SetActive(true);
        if (anomalyState != null) anomalyState.SetActive(false);
    }

}