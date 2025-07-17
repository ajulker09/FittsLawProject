using UnityEngine;
using Oculus.Interaction;

public class ChangeColorOnSelect : MonoBehaviour
{
    private Renderer rend;
    public Color selectedColor = Color.green;
    public Color originalColor;

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
        }
        else
        {
            Debug.LogWarning("Renderer not found on " + gameObject.name);
        }
    }

    public void OnSelect()
    {
        Debug.Log("SELECT CALLED");
        if (rend != null)
            rend.material.color = selectedColor;
    }

    public void OnUnselect()
    {
        Debug.Log("UNSELECT CALLED");
        if (rend != null)
            rend.material.color = originalColor;
    }
}
