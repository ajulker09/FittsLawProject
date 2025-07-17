using UnityEngine;
using Oculus.Interaction;

public class ChangeColorOnSelect : MonoBehaviour
{
    private Renderer rend;
    public Color touchColor = Color.white;
    public Color hitColor = Color.blue;
    private Color originalColor;
    public int id;

    private Vector3 entryPoint;
    private float entryTime;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    public void OnSelect()
    {
        int currentTargetId = CircleRingSpawner.sequence[CircleRingSpawner.currentTargetIndex];

        if (id == currentTargetId)
        {
            entryPoint = transform.position; // Approximate, since ray doesn't have real entry
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
        }
    }
}
