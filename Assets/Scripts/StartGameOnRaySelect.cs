using UnityEngine;
using Oculus.Interaction;

public class StartGameOnRaySelect : MonoBehaviour
{
    public GameObject instructionUI;
    public GameObject circleSpawner;

    private bool gameStarted = false;


    public void OnSelect()
    {
        if (!gameStarted)
        {
            gameStarted = true;

            instructionUI.SetActive(false);
            circleSpawner.SetActive(true);

            Debug.Log("Game Started via Ray Select!");
        }
    }
}
