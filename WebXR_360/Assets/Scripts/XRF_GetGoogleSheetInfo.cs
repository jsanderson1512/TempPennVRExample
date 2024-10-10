using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;
using System.Linq;
using System;
using System.Threading;

public class XRF_GetGoogleSheetInfo : MonoBehaviour
{
    //download test
    //https://docs.google.com/spreadsheets/d/1gseHmqvgk097HVn41hLluQeEiBrECGQ4L55GYhmhN5w/export?format=csv

    [Header("Google Sheet URLs")]
    [Tooltip("This is the END of the URL for the Username and Pin sheet of all employees. To get this, click 'get shareable link', copy the link, and copy the segment BETWEEN '/d/' and '/edit?usp=sharing'.")]
    public string URL_GoogleSheet_ProjectInformation = "https://docs.google.com/spreadsheets/d/1gseHmqvgk097HVn41hLluQeEiBrECGQ4L55GYhmhN5w/edit?usp=sharing";

    private List<string> projectName = new List<string>();
    private List<string> projectDate = new List<string>();
    private List<string> imageURLs = new List<string>();

    private List<string> projectNameUnique = new List<string>();
    private List<string> projectDateUnique = new List<string>();
    private List<List <string>> imageURLsList = new List<List<string>>();


    [Header("Scroll View Properties and References")]
    public float downloadYValueOriginal = -30.0f;
    public float downloadYPadding = 90.0f;
    public GameObject downloadButtonPrefab;
    public GameObject contentHolder;

    private List<GameObject> currentModelList = new List<GameObject>();
    private float downloadYValue = 0.0f;
    private GameObject downloadButton;


    private void Start()
    {
        downloadButtonPrefab.SetActive(false);
    }

    //need a refresh button in case someone uploads while they have the app open
    public void LoadUsernames()
    {
        GetAllUsernames();
    }
    private void GetAllUsernames()
    {
        projectName = new List<string>();
        projectDate = new List<string>();
        imageURLs = new List<string>();

        //this gets the usernams and passwords and locations
        string[] splitURL = URL_GoogleSheet_ProjectInformation.Split("/");
        string k_googleSheetDocID = splitURL[5];
        StartCoroutine(CSVDownloader.DownloadData(k_googleSheetDocID, AfterDownload));
    }

    public void AfterDownload(string data)
    {
        if (null == data)
        {
            Debug.LogError("Was not able to download data or retrieve stale data.");
            // TODO: Display a notification that this is likely due to poor internet connectivity
            //       Maybe ask them about if they want to report a bug over this, though if there's no internet I guess they can't
        }
        else
        {
            StartCoroutine(ProcessData(data));
        }
    }

    public IEnumerator ProcessData(string data)
    {
        yield return new WaitForEndOfFrame();

        // "\r\n" means end of line and should be only occurence of '\r' (unless on macOS/iOS in which case lines ends with just \n)
        char lineEnding = '\r';

#if UNITY_IOS
        lineEnding = '\n';
#endif

#if UNITY_EDITOR
        lineEnding = '\r';
#       endif

        //Debug.Log("data length = " + data.Length);
        string[] dataRows = data.Split(lineEnding);

        string[] rowHeaders = dataRows[0].Split(',');

        int projectNamesInt = 0, projectDatesInt = 0, imageURLsInt = 0;
        for (int i = 0; i < rowHeaders.Length; i++)
        {
            string fixedRowHeader = new string(rowHeaders[i].Where(c => !char.IsControl(c)).ToArray());
            fixedRowHeader = fixedRowHeader.ToLower();

            //Debug.Log("fixedRowHeader = " + fixedRowHeader);

            if (fixedRowHeader == "project name")
            {
                projectNamesInt = i;
            }
            else if (fixedRowHeader == "project date")
            {
                projectDatesInt = i;
            }
            else if (fixedRowHeader == "image url")
            {
                imageURLsInt = i;
            }
        }

        //Debug.Log("studentNamesInt = " + studentNamesInt);
        //Debug.Log("projectNamesInt = " + projectNamesInt);
        //Debug.Log("projectURLsInt = " + projectURLsInt);
        //Debug.Log("diagramURLsInt = " + diagramURLsInt);


        for (int i = 0; i < dataRows.Length; i++)
        {
            string[] dataRowSplit = dataRows[i].Split(',');

            string projectNameOriginal = dataRowSplit[projectNamesInt];
            string projectNameFixed = new string(projectNameOriginal.Where(c => !char.IsControl(c)).ToArray());

            string projectDateOriginal = dataRowSplit[projectDatesInt];
            string projectDateFixed = new string(projectDateOriginal.Where(c => !char.IsControl(c)).ToArray());

            string imageURLOriginal = dataRowSplit[imageURLsInt];
            string imageURLFixed = new string(imageURLOriginal.Where(c => !char.IsControl(c)).ToArray());


            //Debug.Log("im adding this name = " + studentNameFixed);
            //Debug.Log("im adding this project name = " + projectNameFixed);
            //Debug.Log("im adding this project url = " + projectURLFixed);
            //Debug.Log("im adding this diagram url = " + diaramURLFixed);


            projectName.Add(projectNameFixed);
            projectDate.Add(projectDateFixed);
            imageURLs.Add(imageURLFixed);



        }

        //need to run through and get unique ones!
        List<string> tempProjectNameUnique = new List<string>();
        List<List<string>> tempProjectDateUnique = new List<List<string>>();

        projectNameUnique = new List<string>();
        projectDateUnique = new List<string>();
        imageURLsList = new List<List<string>>();

        tempProjectNameUnique = projectName.Distinct().ToList();


        //for each unique name and date combo, i should have a list item all in the same terms...

        //get the distinct dates from each project name as a list
        for (int i = 0; i < tempProjectNameUnique.Count; i++)
        {
            List<string> thisProjectDates = new List<string>();
            for (int j = 0; j < projectName.Count; j++)
            {
                if (tempProjectNameUnique[i] == projectName[j])
                {
                    thisProjectDates.Add(projectDate[j]);
                }
            }
            thisProjectDates = thisProjectDates.Distinct().ToList();
            tempProjectDateUnique.Add(thisProjectDates);
        }

        //get the urls associated with each project name and date combo
        for (int i = 0; i < tempProjectNameUnique.Count; i++)
        {
            for (int j = 0; j < tempProjectDateUnique[i].Count; j++)
            {
                List<string> thisProjectAndDateURLs = new List<string>();
                for (int k = 0; k < projectName.Count; k++)
                {
                    if (tempProjectNameUnique[i] == projectName[k] && tempProjectDateUnique[i][j] == projectDate[k])
                    {
                        thisProjectAndDateURLs.Add(imageURLs[k]);
                    }
                }

                projectNameUnique.Add(tempProjectNameUnique[i]);
                projectDateUnique.Add(tempProjectDateUnique[i][j]);
                imageURLsList.Add(thisProjectAndDateURLs);
            }
        }


        //now i will have a list of project, each has a list of dates associated with it, and each project and date combo has a list of URLs

        PopulateProjectList();
        //populate a scroll view
    }


    void PopulateProjectList()
    {
        for (int i = 0; i < currentModelList.Count; i++)
        {
            Destroy(currentModelList[i]);
        }
        downloadYValue = downloadYValueOriginal;
        currentModelList = new List<GameObject>();


        //start i at 1 because its adding the fixed row header at the top... we dont need this...
        for (int i = 1; i < projectNameUnique.Count; i++)
        {
            downloadButton = Instantiate(downloadButtonPrefab, Vector3.zero, Quaternion.identity);
            //downloadButton.transform.parent = downloadButtonPrefab.transform.parent;
            //downloadButton.GetComponent<RectTransform>().parent = downloadButtonPrefab.GetComponent<RectTransform>().parent;
            RectTransform rt = downloadButton.GetComponent<RectTransform>();
            rt.SetParent(contentHolder.GetComponent<RectTransform>());
            //downloadButton.transform.localPosition = new Vector3(0, downloadYValue, 0);
            rt.localPosition = new Vector3(0, downloadYValue, 0);
            rt.offsetMin = new Vector2(5, rt.offsetMin.y);
            rt.offsetMax = new Vector2(-5, rt.offsetMax.y);

            //downloadButton.transform.localRotation = Quaternion.identity;
            rt.localRotation = Quaternion.identity;

            //downloadButton.transform.localScale = downloadButtonPrefab.transform.localScale;
            rt.localScale = downloadButtonPrefab.GetComponent<RectTransform>().localScale;

            downloadButton.SetActive(true);
            //create a button for each of these and put them into an array
            XRF_ProjectInfoHolder linkComponent = downloadButton.GetComponentInChildren<XRF_ProjectInfoHolder>();
            linkComponent.projectName = projectNameUnique[i];
            linkComponent.projectDate = projectDateUnique[i];
            linkComponent.imageURLs = imageURLsList[i];

            //now we have two...
            TMP_Text textObject = downloadButton.GetComponentInChildren<TMP_Text>();
            textObject.text = projectNameUnique[i] + " - " + projectDateUnique[i];

            currentModelList.Add(downloadButton);
            downloadYValue = downloadYValue - downloadYPadding;




        }


        //content holder height
        Vector2 textSizeDelta = contentHolder.GetComponent<RectTransform>().sizeDelta;
        textSizeDelta.y = (downloadYValue - downloadYPadding) * -1;
        contentHolder.GetComponent<RectTransform>().sizeDelta = textSizeDelta;
    }
}
