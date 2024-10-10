using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class CSVDownloader
{
    internal static IEnumerator DownloadData(string docID, System.Action<string> onCompleted)
    {
        string url = "https://docs.google.com/spreadsheets/d/" + docID + "/export?format=csv";

        Debug.Log("i will download this url: " + url);
        yield return new WaitForEndOfFrame();

        string downloadData = null;
        //Debug.Log("Web Request From: " + url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Download Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Download success");
                downloadData = webRequest.downloadHandler.text;
                Debug.Log("Data: " + downloadData);
            }
        }
        onCompleted(downloadData);
    }
}