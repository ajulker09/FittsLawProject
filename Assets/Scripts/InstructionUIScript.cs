using UnityEngine;
using TMPro;

public class InstructionUIScript : MonoBehaviour
{
    public GameObject instructionCanvas;
    public GameObject spawner;
    public TMP_InputField participantInput;
    public GameObject keyboardCanvas;

    public void StartTest()
    {
        string input = participantInput.text;
        if (!string.IsNullOrEmpty(input))
        {
            ParticipantInfo.ParticipantID = input;
        }

        instructionCanvas.SetActive(false);
        keyboardCanvas.SetActive(false);
        spawner.SetActive(true);
    }
}
