using TMPro;
using UnityEngine;
using System.Collections;

public class CountDownTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public int startTime = 3;

    public void StartCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        int currentTime = startTime;

        while (currentTime >= 0)
        {
            timerText.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        timerText.text = ""; 
    }
}
