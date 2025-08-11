using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartGameAndShowInstruction : MonoBehaviour
{
    public GameObject experiment;

    public GameObject setupUI;
    public GameObject instructionUI;
    public Toggle toggle;

    public TMP_InputField participantInput;
    public TMP_Text instructionText;
    public TMP_Text header;

    private bool gameStarted = false;

    private void OnTriggerEnter(Collider other)
    {

        
        if (gameStarted) return;

        string input = participantInput.text;
        if (!string.IsNullOrEmpty(input))
        {
            ExperimentManager manager = experiment.GetComponent<ExperimentManager>();

            Debug.Log("Participant ID: " + input);

            int pid = -1;
            int.TryParse(input, out pid);
            manager.SetupParticipant(pid, toggle.isOn);


            ShowInstruction(manager);

            gameStarted = true;
        }
    }

    private void ShowInstruction(ExperimentManager manager)
    {
        setupUI.SetActive(false);

        ExperimentManager.Condition condition = manager.GetCondition();

        switch (condition)
        {
            case ExperimentManager.Condition.None:
                instructionText.text =
                    "- Touch the green circle as quickly and accurately as possible\n" +
                    "- After each successful touch, a new circle will appear\n" +
                    "- You will not receive any feedback for whether you have touched the target\n" +
                    "- A new target will appear once you stop touching the original target\n" +
                    "- Complete all trials to finish the task\n" +
                    "- Try not to rest until the rest screen appears\n";
                header.text = "INSTRUCTIONS: No Feedback";
                break;

            case ExperimentManager.Condition.Dominant:
                instructionText.text =
                    "- Touch the green circle as quickly and accurately as possible\n" +
                    "- After each successful touch, a new circle will appear\n" +
                    "- The watch will vibrate when you successfully touch the target\n" +
                    "- A new target will appear once you stop touching the original target\n" +
                    "- Complete all trials to finish the task\n" +
                    "- Try not to rest until the rest screen appears\n";
                header.text = "INSTRUCTIONS: Dominant Vibration";
                break;

            case ExperimentManager.Condition.Visual:
                instructionText.text =
                    "- Touch the green circle as quickly and accurately as possible\n" +
                    "- After each successful touch, a new circle will appear\n" +
                    "- The target will change colour when you successfully touch it\n" +
                    "- A new target will appear once you stop touching the original target\n" +
                    "- Complete all trials to finish the task\n" +
                    "- Try not to rest until the rest screen appears\n";
                header.text = "INSTRUCTIONS: Visual";
                break;

            case ExperimentManager.Condition.NonDominant:
                instructionText.text =
                    "- Touch the green circle as quickly and accurately as possible\n" +
                    "- After each successful touch, a new circle will appear\n" +
                    "- The watch will vibrate when you successfully touch the target\n" +
                    "- A new target will appear once you stop touching the original target\n" +
                    "- Complete all trials to finish the task\n" +
                    "- Try not to rest until the rest screen appears\n";
                header.text = "INSTRUCTIONS: NonDominant";
                break;
        }

        instructionUI.SetActive(true);
        Debug.Log("Instructions displayed.");
    }
}
