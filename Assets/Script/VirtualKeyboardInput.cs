using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VirtualKeyboardInput : MonoBehaviour
{
    public TMP_InputField inputField;
    public string character;

    private Image image;
    private Color originalColor;
    public Color pressedColor = Color.gray;

    private float lastTriggerTime = 0f;
    public float debounceDelay = 1f; 

    private void Start()
    {
        image = GetComponent<Image>();
        if (image != null)
        {
            originalColor = image.color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastTriggerTime < debounceDelay)
            return;

        lastTriggerTime = Time.time;

        if (character == "Back")
        {
            if (inputField.text.Length > 0)
            {
                inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
            }
        }
        else
        {
            inputField.text += character;
        }



        if (image != null)
        {
            image.color = pressedColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (image != null)
        {
            image.color = originalColor;
        }
    }
}
