using UnityEngine;
using TMPro;

public class GoToInstruction : MonoBehaviour
{
    public GameObject CalibrateCanvas;
    public GameObject InsCanvas;
    public CalibrateButtonTrigger calibrateButtonTrigger;
    public TMP_Text instructionText;
    public TMP_Text header;

    private int counter = 0;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(counter);
        calibrateButtonTrigger.ResetUI();
        CalibrateCanvas.SetActive(false);
        InsCanvas.SetActive(true);

        if (counter == 0) 

        {
            instructionText.text =
                "- Touch the highlighted circle as quickly and accurately as possible\n" +
                "- After each successful touch, a new circle will appear\n" +
                "- Wait for the circle to light up before reaching toward it\n" +
                "- Only one circle will be active at a time\n" +
                "- Try to maintain a natural and steady movement without jerky motions\n" +
                "- The task will automatically continue to the next target after a correct touch\n" +
                "- Complete all targets to finish the task";

            header.text = "INSTRUCTIONS: Finger Touch";
        }
        else if (counter == 1)
        {
            instructionText.text =
                "- Touch the highlighted circle as quickly and accurately as possible\n" +
                "- Your watch will vibrate when the circle is ready to be touched\n" +
                "- After each successful touch, a new circle will appear\n" +
                "- Wait for the vibration before reaching toward the circle\n" +
                "- Only one circle will be active at a time\n" +
                "- Try to maintain a natural and steady movement without jerky motions\n" +
                "- The task will automatically continue to the next target after a correct touch\n" +
                "- Complete all targets to finish the task";
            header.text = "INSTRUCTIONS: Finger Touch with Watch";
        }

        counter++;
    }
}
