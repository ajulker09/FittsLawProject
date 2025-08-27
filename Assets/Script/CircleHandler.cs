using UnityEngine;
using Oculus.Interaction;
using System.Collections;

public class ChangeColorOnTouch : MonoBehaviour
{
    private Renderer rend;

    public Color touchColor = Color.white;
    public Color hitColor = Color.blue;
    private Color originalColor;

    public int id;

    private Vector3 entryPoint;
    private float entryTime;

    public bool hasBeenClicked = false;
    private CircleRingSpawner circleRingSpawner;
    

    void Awake()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color; // only set once here
        circleRingSpawner = Object.FindFirstObjectByType<CircleRingSpawner>();
    }

    
    public void SetOriginalColor(Color c)
    {
        originalColor = c;
    }

    public void OnHover()
    {
        if (!hasBeenClicked && circleRingSpawner.getCondition() == "Visual" )
        {
            rend.material.color = touchColor;
        }
        
        Debug.Log("In OnHover and has been clicked: " + hasBeenClicked);
        
        circleRingSpawner.EnterFeedback(id);
    }

    public void OnUnhover()
    {
        if (!hasBeenClicked)
        {
            rend.material.color = originalColor;
        }
        hasBeenClicked = false;
        Debug.Log("In OnUnHover and has been clicked: " + hasBeenClicked);
    }

    public void OnSelect()
    {
        int currentTargetId = CircleRingSpawner.sequence[CircleRingSpawner.currentTargetIndex];
        Debug.Log($"[SELECT] Circle ID: {id}, Target ID: {currentTargetId}");

        if (id == currentTargetId)
        {
            hasBeenClicked = true;
            entryPoint = transform.position;
            entryTime = Time.time;
            
        }
    }

    public void OnUnselect()
    {
        int currentTargetId = CircleRingSpawner.sequence[CircleRingSpawner.currentTargetIndex];

        if (id == currentTargetId)
        {
            Vector3 exitPoint = transform.position;
            float exitTime = Time.time;


            rend.material.color = hitColor;
            originalColor = hitColor; 



            CircleRingSpawner spawner = FindObjectOfType<CircleRingSpawner>();
            spawner.RecordTouch(entryPoint, exitPoint, entryTime, exitTime);
            spawner.AdvanceToNextTarget();

            StartCoroutine(ResetClickAfterDelay(1f));
        }
    }

    private IEnumerator ResetClickAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hasBeenClicked = false;
    }
}
