using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebXR;


public class StartVR : MonoBehaviour
{
    private WebXRManager manager;

    public void TryStartVR()
    {
        if(manager == null)
        {
            GameObject gO = GameObject.Find("WebXRManager");
            manager = gO.GetComponent<WebXRManager>();
        }

        if (manager != null)
        {
            Debug.Log("try start vr");
            manager.ToggleVR();
        }
    }
}