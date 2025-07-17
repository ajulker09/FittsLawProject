using UnityEngine;

public class FingerTracker : MonoBehaviour
{
    public Transform rightIndexTip;
    public Transform xrOrigin;

    public float FinalReach { get; private set; }
    public float FinalHeight { get; private set; }

    private float lastDistance = 0f;
    private float stableStartTime = 0f;
    private bool distanceCaptured = false;

    private float stabilityThreshold = 0.01f;
    private float requiredDistance = 0.5f;
    private float requiredStabilityTime = 3f;

    void Update()
    {
        if (rightIndexTip == null || xrOrigin == null || distanceCaptured)
            return;

        float currentDistance = Vector3.Distance(xrOrigin.position, rightIndexTip.position);

        if (currentDistance >= requiredDistance)
        {
            float distanceChange = Mathf.Abs(currentDistance - lastDistance);

            if (distanceChange < stabilityThreshold)
            {
                if (stableStartTime == 0f)
                    stableStartTime = Time.time;

                if (Time.time - stableStartTime >= requiredStabilityTime)
                {
                    FinalReach = currentDistance;
                    FinalHeight = rightIndexTip.position.y;

                    Debug.Log("Final Reach: " + FinalReach.ToString("F3"));
                    Debug.Log("Final Height: " + FinalHeight.ToString("F3"));
                    Debug.Log("YEASSSASD");

                    distanceCaptured = true;
                }
            }
            else
            {
                stableStartTime = 0f;
            }

            lastDistance = currentDistance;
        }
        else
        {
                    stableStartTime = 0f;
            lastDistance = currentDistance;
        }
    }
}
