using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PhoneAppManager : MonoBehaviour
{
    [Header("Hardware References")]
    public GameObject phoneObject;
    public Transform phoneModelTransform;
    public Camera photoCamera;
    public PhotoSystem photoSystem;

    [Header("Rotation Settings")]
    public Vector3 horizontalRotation = new Vector3(0, 0, 90);
    public Vector3 verticalRotation = Vector3.zero;

    [Header("UI Panels")]
    public GameObject homePanel;
    public GameObject cameraPanel;
    public GameObject linePanel;
    public GameObject photoSelectionPopup;
    public GameObject confirmButton;

    [Header("Gallery UI")]
    public GameObject thumbnailPrefab;
    public Transform galleryContent;
    public RawImage fullScreenDisplay;

    private List<Texture2D> capturedPhotos = new List<Texture2D>();
    private List<bool> photoResults = new List<bool>();
    private bool isPhoneOpen = false;
    private bool hasSentAnyPhoto = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) TogglePhone();

        if (isPhoneOpen && cameraPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(CapturePhotoRoutine());
        }
    }

    IEnumerator CapturePhotoRoutine()
    {
        yield return new WaitForEndOfFrame();

        RenderTexture rt = photoCamera.targetTexture;
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        screenShot.Apply();

        capturedPhotos.Add(screenShot);

        bool isHit = CheckAnomalyWithRaycast();
        photoResults.Add(isHit);

        Debug.Log("ถ่ายรูปสำเร็จ! ติด Anomaly: " + isHit);
    }

    bool CheckAnomalyWithRaycast()
    {
        float detectionRadius = 3.0f; // ขยายรัศมีให้กว้างขึ้นอีก
        float maxDistance = 50f;

        Vector3 origin = photoCamera.transform.position;
        Vector3 direction = photoCamera.transform.forward;

        // ยิง SphereCast (ลำแสงทรงกระบอก)
        RaycastHit[] hits = Physics.SphereCastAll(origin, detectionRadius, direction, maxDistance);

        foreach (RaycastHit hit in hits)
        {
            // ตรวจสอบทั้งตัวมันเอง และ Object ที่อยู่รอบๆ (Parent/Children)
            if (hit.collider.CompareTag("Anomaly") || hit.collider.CompareTag("Fixed"))
            {
                Debug.Log("<color=green>ตรวจพบเป้าหมาย!</color> โดนวัตถุ: " + hit.collider.gameObject.name);
                return true;
            }
        }
        return false;
    }

    public void OpenGallery()
    {
        photoSelectionPopup.SetActive(true);
        fullScreenDisplay.gameObject.SetActive(false);
        if (confirmButton != null) confirmButton.SetActive(false);

        foreach (Transform child in galleryContent) Destroy(child.gameObject);

        for (int i = 0; i < capturedPhotos.Count; i++)
        {
            int index = i;
            GameObject btn = Instantiate(thumbnailPrefab, galleryContent);
            btn.GetComponent<RawImage>().texture = capturedPhotos[index];

            btn.GetComponent<Button>().onClick.AddListener(() => {
                fullScreenDisplay.gameObject.SetActive(true);
                fullScreenDisplay.texture = capturedPhotos[index];
                if (confirmButton != null) confirmButton.SetActive(true);

                photoSystem.isAnomalyCaptured = photoResults[index];
            });
        }
    }

    public void ClearAllPhotos()
    {
        capturedPhotos.Clear();
        photoResults.Clear();
        hasSentAnyPhoto = false;
        if (photoSystem != null) photoSystem.isAnomalyCaptured = false;
        foreach (Transform child in galleryContent) Destroy(child.gameObject);
    }

    public void OpenCamera()
    {
        SetAllInactive();
        cameraPanel.SetActive(true);
        phoneModelTransform.localEulerAngles = horizontalRotation; 
    }

    public void OpenHome()
    {
        SetAllInactive();
        homePanel.SetActive(true);
        phoneModelTransform.localEulerAngles = verticalRotation; 
    }

    public void OpenLine()
    {
        SetAllInactive();
        linePanel.SetActive(true);
        phoneModelTransform.localEulerAngles = verticalRotation;
    }

    public void TogglePhone()
    {
        isPhoneOpen = !isPhoneOpen;
        phoneObject.SetActive(isPhoneOpen);
        Cursor.lockState = isPhoneOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPhoneOpen;
        if (isPhoneOpen) OpenHome();
    }

    public void ConfirmAndSend()
    {
        hasSentAnyPhoto = true;
        photoSelectionPopup.SetActive(false);
    }

    public bool DidUserSendPhoto() => hasSentAnyPhoto;

    void SetAllInactive()
    {
        homePanel.SetActive(false);
        cameraPanel.SetActive(false);
        linePanel.SetActive(false);
        photoSelectionPopup.SetActive(false);
    }
}