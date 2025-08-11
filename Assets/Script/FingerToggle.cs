using UnityEngine;
using UnityEngine.UI;

public class FingerToggle : MonoBehaviour
{
    private Toggle toggle;
    private bool isTouching = false;

    void Start()
    {
        toggle = transform.parent.parent.gameObject.GetComponent<Toggle>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (!isTouching) {
            isTouching = true;
            toggle.isOn = !toggle.isOn;
        
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        isTouching = false;
    }
}
