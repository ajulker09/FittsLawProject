using UnityEngine;
using TMPro;

public class CalibrateButtonTrigger : MonoBehaviour
{
    public GameObject calibrateText;
    public GameObject calibrateButton;
    public GameObject calibrateText2;
    public GameObject calibrateText3;
    public GameObject FingerTracking;
    public GameObject NextButton;
    public TMP_Text timerText; 

    private int countdownValue = 5;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("IAM HIT");
        calibrateText.SetActive(false);
        calibrateButton.SetActive(false);
        calibrateText2.SetActive(true);
        FingerTracking.SetActive(true);
        timerText.gameObject.SetActive(true); 
        

        countdownValue = 3;
        timerText.text = countdownValue.ToString();
        InvokeRepeating("UpdateCountdown", 1f, 1f);
    }

    private void UpdateCountdown()
    {
        countdownValue--;
        timerText.text = countdownValue.ToString();

        if (countdownValue < 0)
        {
            CancelInvoke("UpdateCountdown");
            timerText.gameObject.SetActive(false);  
            calibrateText3.SetActive(true);
            calibrateText2.SetActive(false);
            NextButton.SetActive(true);
            

        } 
    }

    public void ResetUI()
    {
        calibrateText3.SetActive(false);
        NextButton.SetActive(false); 
        calibrateText.SetActive(true);
        calibrateButton.SetActive(true); 
    }
}
