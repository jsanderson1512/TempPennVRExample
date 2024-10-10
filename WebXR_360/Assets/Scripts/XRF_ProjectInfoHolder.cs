using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class XRF_ProjectInfoHolder : MonoBehaviour
{
    [Header("Reference to Game Manager Objects")]
    public PanoramaSwitchHandler panoramaHolder;
    public XRF_CanvasController canvasControl;

    [Header("Reference to Canvas Items")]
    public GameObject LoadingScreen;
    public TMP_Text LoadingScreenText;
    public GameObject DownloadScreen;

    [Header("Project Information")]
    public string projectName;
    public string projectDate;
    public List<string> imageURLs;

    private List<Texture> allTextures2D = new List<Texture>();

    private void Start()
    {
        LoadingScreen.SetActive(false);
    }
    public void DownloadThisProject()
    {
        LoadingScreenText.text = "Loading Images...";
        LoadingScreen.SetActive(true);
        StartCoroutine(DownloadAllFiles());
    }

    public IEnumerator DownloadAllFiles()
    {
        for (int i = 0; i < imageURLs.Count; i++)
        {
            //trying with firebase locations...

            /*
            LoadingScreenText.text = "Loading Image " + (i+1).ToString() + " of " + imageURLs.Count;
            string[] splitURLImage = imageURLs[i].Split("/");
            string k_imageDocID = splitURLImage[5];
            */

            string k_imageDocID = imageURLs[i];

            yield return StartCoroutine(TextureDownloader.DownloadData(k_imageDocID, AfterDownloadImage));
        }
        SendImagesToPanoramaHolder();
        TryTurnOffInterface();
    }

    public void AfterDownloadImage(Texture theDownload)
    {
        Texture tex2D = (theDownload as Texture); // where tex is of type 'Texture'
        allTextures2D.Add(tex2D);
    }
    public void SendImagesToPanoramaHolder()
    {
        panoramaHolder.panoramaTexturesFlat = allTextures2D.ToArray();
        panoramaHolder.SwitchPanorama();
    }
    public void TryTurnOffInterface()
    {
        canvasControl.somethingIsLoaded = true;
        canvasControl.ReturnToStart();
        LoadingScreen.SetActive(false);
        DownloadScreen.SetActive(false);
    }
}
