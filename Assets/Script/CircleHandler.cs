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

    void Awake()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    public void OnHover()
    {
        if (!hasBeenClicked)
        {
            originalColor = rend.material.color;
            rend.material.color = touchColor;
        }
    }

    public void OnUnhover()
    {
        if (!hasBeenClicked)
        {
            rend.material.color = originalColor;
        }
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
            rend.material.color = touchColor;
        }
    }

    public void OnUnselect()
    {
        int currentTargetId = CircleRingSpawner.sequence[CircleRingSpawner.currentTargetIndex];

        if (id == currentTargetId)
        {
            Vector3 exitPoint = transform.position;
            float exitTime = Time.time;

            CircleRingSpawner spawner = FindObjectOfType<CircleRingSpawner>();
            spawner.RecordTouch(entryPoint, exitPoint, entryTime, exitTime);
            spawner.AdvanceToNextTarget();

            rend.material.color = hitColor;
            originalColor = hitColor;

            
            StartCoroutine(ResetClickAfterDelay(1f));
        }
    }

    private IEnumerator ResetClickAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hasBeenClicked = false;
    }
}
