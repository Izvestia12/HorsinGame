using UnityEngine;

public class ItemSnapPoint : MonoBehaviour
{
    public string targetItemName = "Artwork_Label";
    public GameObject realLabel;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == targetItemName)
        {
            SnapObject(other.gameObject);
        }
    }

    void SnapObject(GameObject item)
    {
        Destroy(item);

        if (realLabel != null)
            realLabel.SetActive(true);

        Debug.Log("วางป้ายเข้าที่แล้ว!");

        // ส่งสัญญาณไปที่สคริปต์หลัก
        LabelAnomalyLogic logic = GetComponentInParent<LabelAnomalyLogic>();
        if (logic != null)
        {
            logic.MarkAsFixed();
        }
    }
}