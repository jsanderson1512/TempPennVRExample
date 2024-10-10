using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRF_CanvasController : MonoBehaviour
{
    public GameObject nothingLoadedInterface;
    public GameObject startButtons;
    public GameObject importLoadedInterface;
    public GameObject projectInterface;
    public GameObject loadingScreen;

    public bool somethingIsLoaded = false;

    void Start()
    {
        startButtons.SetActive(true);

        projectInterface.SetActive(false);
        loadingScreen.SetActive(false);

        ReturnToStart();

    }


    public void DownloadInterfaceOn()
    {
        projectInterface.SetActive(true);

    }

    public void DownloadInterfaceOff()
    {
        projectInterface.SetActive(false);

    }

    public void LoadingPanelOff()
    {
        loadingScreen.SetActive(false);
    }


    public void ReturnToStart()
    {

        if (somethingIsLoaded)
        {
            nothingLoadedInterface.SetActive(false);
            importLoadedInterface.SetActive(true);

        }
        else
        {
            nothingLoadedInterface.SetActive(true);
            importLoadedInterface.SetActive(false);
        }
    }
}
