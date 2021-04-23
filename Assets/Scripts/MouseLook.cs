using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    // Update is called once per frame
    void Update()
    {
        var deltaTime = Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);


        var weapon = transform.parent.GetComponentInChildren<Shotgun>();
        weapon.transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2)) + Camera.main.transform.forward * 50);

    }


}
