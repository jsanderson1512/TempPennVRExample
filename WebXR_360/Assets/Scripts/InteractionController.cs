using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionController : MonoBehaviour
{
    public UnityEvent theEvent;
    private bool clicked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!clicked)
        {
            theEvent.Invoke();
            clicked = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        clicked = false;
    }
}
