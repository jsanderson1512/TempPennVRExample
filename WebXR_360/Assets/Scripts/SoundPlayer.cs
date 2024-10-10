using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource theSource;
    public void PlaySound()
    {
        theSource.Play(0);
    }
}
