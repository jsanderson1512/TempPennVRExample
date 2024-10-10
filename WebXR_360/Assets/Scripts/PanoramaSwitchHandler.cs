using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PanoramaSwitchHandler : MonoBehaviour
{
    public Material SkyboxMono;
    public Material SkyboxStereo;

    //public MeshRenderer sphere;

    public Texture[] panoramaTexturesFlat;

    //private Material theSkybox;
    private Texture currentPanoramaTexture;
    private Texture leftPanoramaTexture;
    private Texture rightPanoramaTexture;

    private int selectionCounter = 5000;

    public MeshRenderer leftSelection;
    public MeshRenderer rightSelection;


    // Start is called before the first frame update
    void Start()
    {
        //theSkybox = RenderSettings.skybox;
        SwitchPanorama();
    }


    public void Button_Left()
    {
        selectionCounter--;
        SwitchPanorama();
    }
    public void Button_Right()
    {
        selectionCounter++;
        SwitchPanorama();
    }


    public void SwitchPanorama()
    {
        if (panoramaTexturesFlat.Length > 0)
        {
            int selectionNumber = selectionCounter % panoramaTexturesFlat.Length;
            currentPanoramaTexture = panoramaTexturesFlat[selectionNumber];
            
            //previously, i used this sphere around the player with a 360 renderer on it, for some reason...
            //theSkybox.SetTexture("_Tex", currentPanoramaTexture);
            //sphere.material.mainTexture = currentPanoramaTexture;


            //determine if our image is mono or stereo and set the skybox appropriately
            if (currentPanoramaTexture.width == currentPanoramaTexture.height)
            {
                //if these are equal, we have a stereo image
                SkyboxStereo.SetTexture("_MainTex", currentPanoramaTexture);
                RenderSettings.skybox = SkyboxStereo;
            }
            else
            {
                SkyboxMono.SetTexture("_MainTex", currentPanoramaTexture);
                RenderSettings.skybox = SkyboxMono;
            }


            int leftInt = (selectionCounter - 1) % panoramaTexturesFlat.Length;
            leftPanoramaTexture = panoramaTexturesFlat[leftInt];
            leftSelection.material.mainTexture = leftPanoramaTexture;

            int rightInt = (selectionCounter + 1) % panoramaTexturesFlat.Length;
            rightPanoramaTexture = panoramaTexturesFlat[rightInt];
            rightSelection.material.mainTexture = rightPanoramaTexture;
        }
    }
}
