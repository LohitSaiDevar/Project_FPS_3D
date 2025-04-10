using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 500;
    float xRotation;
    float yRotation;

    [SerializeField] float topClamp = -90;
    [SerializeField] float bottomClamp = 90;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        xRotation -= mouseY * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        yRotation += mouseX * mouseSensitivity * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
