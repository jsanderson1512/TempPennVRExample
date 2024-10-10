using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TextureDownloader 
{

    internal static IEnumerator DownloadData(string docID, System.Action<Texture> onCompleted)
    {
        //trying with firebase locations...
        //string url = "https://drive.google.com/uc?export=download&id=" + docID;

        string url = docID;

        Debug.Log("i will download this url: " + url);
        yield return new WaitForEndOfFrame();

        //Debug.Log("Web Request From: " + url);

        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;

            onCompleted(myTexture);
        }
    }
}
