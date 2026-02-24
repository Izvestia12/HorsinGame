using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Sway Settings")]
    public float speed = 3.0f;
    public float amount = 0.05f;

    private Quaternion originRotation;

    void Start()
    {
        originRotation = transform.localRotation;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * amount;
        float mouseY = Input.GetAxis("Mouse Y") * amount;

        Quaternion targetRotation = Quaternion.Euler(mouseY, -mouseX, 0);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation * originRotation, Time.deltaTime * speed);
    }
}
