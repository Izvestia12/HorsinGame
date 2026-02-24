using UnityEngine;

public class WeepingAngel : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform player;
    public float moveSpeed = 4.5f;
    public Camera playerCamera;

    [Header("QTE Settings")]
    public float interactDistance = 2.5f;
    public GameObject qtePrompt;

    private bool isBeingWatched;
    private bool isPushing = false;

    void Start()
    {
        // บังคับแปะป้าย Anomaly ทันทีที่เริ่มเกม
        this.tag = "Anomaly";

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (isPushing || player == null) return;

        isBeingWatched = CheckIfWatched();

        if (!isBeingWatched)
        {
            MoveAndAvoid();
        }

        HandleInteraction();
    }

    void MoveAndAvoid()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        RaycastHit hit;
        // ยิง Ray เช็คสิ่งกีดขวางข้างหน้า (หนวดแมว)
        bool obstacleAhead = Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 1.5f);

        if (obstacleAhead && !hit.collider.CompareTag("Player"))
        {
            // ถ้าเจอของขวาง ให้เบี่ยงทิศทางออกไปทางขวา
            direction += transform.right * 1.5f;
        }

        // ขยับ
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        // หันหน้า
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
        }
    }

    bool CheckIfWatched()
    {
        Vector3 screenPoint = playerCamera.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (onScreen)
        {
            RaycastHit hit;
            if (Physics.Linecast(playerCamera.transform.position, transform.position, out hit))
            {
                if (hit.transform == transform || hit.transform.IsChildOf(transform)) return true;
            }
        }
        return false;
    }

    void HandleInteraction()
    {
        // 1. เช็คระยะห่างระหว่างผู้เล่นกับรูปปั้น
        float dist = Vector3.Distance(transform.position, player.position);

        // 2. เช็คทิศทาง (Dot Product) 
        // ค่า Dot < 0 คืออยู่ด้านหลัง / ยิ่งเข้าใกล้ -1 คือยิ่งอยู่หลังตรงๆ
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dirToPlayer);

        // ปรับค่า dot เป็น < 0 (แปลว่าขอแค่ครึ่งวงกลมด้านหลังทั้งหมด) จะได้กดง่ายขึ้น
        if (dist <= interactDistance && dot < 0)
        {
            if (qtePrompt != null && !qtePrompt.activeSelf)
            {
                qtePrompt.SetActive(true);
                Debug.Log("เข้าใกล้ด้านหลังรูปปั้นแล้ว: กด E ได้!");
            }

            if (Input.GetKeyDown(KeyCode.E)) StartQTE();
        }
        else
        {
            if (qtePrompt != null && qtePrompt.activeSelf) qtePrompt.SetActive(false);
        }
    }

    void StartQTE()
    {
        isPushing = true;
        if (qtePrompt != null) qtePrompt.SetActive(false);

        PushQTE qte = GetComponent<PushQTE>();
        if (qte != null) qte.Begin(this);
    }

    public void OnFallDown()
    {
        Debug.Log("รูปปั้นล้มแล้ว!");
        transform.Rotate(-90, 0, 0);
        this.tag = "Untagged"; // ถ่ายรูปไม่ติดแล้ว
        this.enabled = false;
    }
}