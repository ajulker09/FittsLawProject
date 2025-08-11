using UnityEngine;

public class Continue : MonoBehaviour
{
    public GameObject spawner;
    public GameObject currentUI;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject experiment;

    private void OnTriggerEnter(Collider other)
    {

        ExperimentManager manager = experiment.GetComponent<ExperimentManager>();
        manager = experiment.GetComponent<ExperimentManager>(); 



        currentUI.SetActive(false);
        spawner.SetActive(true);


        if (manager.IsLeftHand())
        {
            var gizmo = leftHand.GetComponent("RayInteractorDebugGizmos");
            if (gizmo != null)
            {
                ((MonoBehaviour)gizmo).enabled = true;
            }
        }
        else
        {
            var gizmo = rightHand.GetComponent("RayInteractorDebugGizmos");
            if (gizmo != null)
            {
                ((MonoBehaviour)gizmo).enabled = true;
            }
        }
            
       
    }
}
