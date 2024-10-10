using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    //public variables
    public GameObject cameraRig;
    public float mouseSensitivity = 0.25f;

    //private variables
    private Vector3 lastMouseRot = new Vector3(255, 255, 255);
    private Vector3 lastMousePitch = new Vector3(255, 255, 255);
    private bool mouseClicked = false;

    private void Update()
    {
        //HANDLE ROTATION BELOW >>>
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            mouseClicked = true;
        }
        else if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
        {
            mouseClicked = false;
        }

        //if my mouse is clicked, start from the click point and rotate from there
        //if my mouse is unClicked, don't rotate at all

        if (mouseClicked)
        {
            //rotation of game object
            lastMouseRot = Input.mousePosition - lastMouseRot;
            lastMouseRot = new Vector3(0.0f, lastMouseRot.x * mouseSensitivity, 0.0f);
            lastMouseRot = new Vector3(0.0f, cameraRig.transform.eulerAngles.y + lastMouseRot.y, 0.0f);
            //mainCamera.transform.eulerAngles = lastMouseRot;

            //pitch of head
            lastMousePitch = Input.mousePosition - lastMousePitch;
            lastMousePitch = new Vector3(-lastMousePitch.y * mouseSensitivity, 0.0f, 0.0f);
            lastMousePitch = new Vector3(cameraRig.transform.eulerAngles.x + Mathf.Clamp(lastMousePitch.x, -89, 89), 0.0f, 0.0f);
            //mainCamera.transform.localEulerAngles = lastMousePitch;

            //Vector3 cameraRotation = lastMouseRot + lastMousePitch;
            //mainCamera.transform.eulerAngles = cameraRotation;
            Vector3 finalRotation = lastMouseRot + lastMousePitch;
            cameraRig.transform.eulerAngles = finalRotation;
        }
        lastMouseRot = Input.mousePosition;
        lastMousePitch = Input.mousePosition;
    }
}
