using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject spawner;
    public GameObject calibrateCanvas;

    private void OnTriggerEnter(Collider other)
    {
        
        calibrateCanvas.SetActive(false);
        spawner.SetActive(true);
        
    }

}
