using UnityEngine;
using TMPro;

public class GoToInstruction : MonoBehaviour
{

    public GameObject experiment;

    public GameObject currentUI;
    public GameObject instructionUI;

    public TMP_Text instructionText;
    public TMP_Text header;
    

    private void OnTriggerEnter(Collider other)
    {


        ExperimentManager manager = experiment.GetComponent<ExperimentManager>();

        currentUI.SetActive(false);

        Debug.Log("PID: " + manager.GetPID());
        Debug.Log("Condition Index: " + manager.GetConditionIndex());
        Debug.Log("Condition" + manager.GetCondition());

        ExperimentManager.Condition condition = manager.GetCondition();

        if (condition == ExperimentManager.Condition.None) 

        {
            instructionText.text =
                "- Touch the green circle as quickly and accurately as possible\n" +
                "- After each successful touch, a new circle will appear\n" +
                "- You will not receive any feedback for whether you have touched the target\n" +
                "- A new target will appear once you stop touching the original target\n" +
                "- Complete all trials to finish the task\n" + 
                "- Try not to rest until the rest screen appears\n";

            header.text = "INSTRUCTIONS: No Feedback";
        }
        else if (condition == ExperimentManager.Condition.Dominant)
        {
            instructionText.text =
                "- Touch the green circle as quickly and accurately as possible\n" +
                "- After each successful touch, a new circle will appear\n" +
                "- The watch will vibrate when you successfully touch the target\n" +
                "- A new target will appear once you stop touching the original target\n" +
                "- Complete all trials to finish the task\n" + 
                "- Try not to rest until the rest screen appears\n";

            header.text = "INSTRUCTIONS: Dominant Vibration";
        }
        else if (condition == ExperimentManager.Condition.Visual)
        {
            instructionText.text =
                "- Touch the green circle as quickly and accurately as possible\n" +
                "- After each successful touch, a new circle will appear\n" +
                "- The target will change colour when you successfully touch it\n" +
                "- A new target will appear once you stop touching the original target\n" +
                "- Complete all trials to finish the task\n" + 
                "- Try not to rest until the rest screen appears\n";

            header.text = "INSTRUCTIONS: Visual";
        }
        else if (condition == ExperimentManager.Condition.NonDominant)
        {
            instructionText.text =
                "- Touch the green circle as quickly and accurately as possible\n" +
                "- After each successful touch, a new circle will appear\n" +
                "- The watch will vibrate when you successfully touch the target\n" +
                "- A new target will appear once you stop touching the original target\n" +
                "- Complete all trials to finish the task\n" + 
                "- Try not to rest until the rest screen appears\n";

            header.text = "INSTRUCTIONS: NonDominant";
        }


        instructionUI.SetActive(true);
    }

 
}
