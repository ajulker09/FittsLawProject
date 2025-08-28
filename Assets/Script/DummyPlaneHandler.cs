using UnityEngine;
using Oculus.Interaction;
using System.Collections;

public class DummyPlaneHandler : MonoBehaviour
{



    public void OnSelectEvent()
    {
        Debug.Log("Selecting");

    }

    public void OnUnselectEvent()
    {
        CircleRingSpawner circleRingSpawner = Object.FindFirstObjectByType<CircleRingSpawner>();
        circleRingSpawner.IncrementSelectAttempts();
        Debug.Log("~~~~~~~~ " + circleRingSpawner.GetSelectAttempts());
    }


 }
