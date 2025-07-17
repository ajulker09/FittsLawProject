using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class CircleRingSpawner : MonoBehaviour
{
    public GameObject circlePrefab;
    public GameObject Spawner;
    public int numberOfCircles = 10;
    public Color newColor = Color.green;

    // Replacing FingerTracker
    public float FinalHeight = -0.22f; // adjust as needed
    public float FinalReach = 0.7f;    // adjust as needed

    public static int currentTargetIndex = 0;
    public static List<int> sequence = new List<int> { 0, 6, 12, 5, 11, 4, 10, 3, 9, 2, 8, 1, 7 };

    private float reactionStartTime;
    private List<GameObject> circles = new List<GameObject>();
    private List<Vector2> testPairs = new List<Vector2> {
        new Vector2(0.30f, 0.35f)
    };

    private int currentTestIndex = 0;
    private float radius;
    public Vector3 center;

    private int currentBlock = 1;
    public int totalBlocks = 2;

    private List<int> targetIDs = new List<int>();
    private List<float> reactionTimes = new List<float>();
    private List<Vector3> targetPositions = new List<Vector3>();
    private List<Vector3> enterPositions = new List<Vector3>();
    private List<Vector3> exitPositions = new List<Vector3>();
    private List<float> trialStartTimes = new List<float>();
    private List<float> timeEnter = new List<float>();
    private List<float> timeExit = new List<float>();

    void OnEnable()
    {
        sequence = GenerateAcrossSequence(numberOfCircles);
        StartNextTest();
    }

    private List<int> GenerateAcrossSequence(int total)
    {
        List<int> sequence = new List<int>();
        int current = 0;
        int halfway = (total + 1) / 2;

        for (int i = 0; i < total; i++)
        {
            sequence.Add(current);
            current = (current + halfway) % total;
        }

        return sequence;
    }

    private void StartNextTest()
    {
        if (currentTestIndex >= testPairs.Count)
        {
            if (currentBlock < totalBlocks)
            {
                Debug.Log("Block " + currentBlock + " completed.");
                currentBlock++;
                currentTestIndex = 0;

                Spawner.SetActive(false);
                return;
            }
            else
            {
                Debug.Log("Final block " + currentBlock + " completed.");
                Spawner.SetActive(false);
                return;
            }
        }

        radius = testPairs[currentTestIndex].x;
        center = new Vector3(0f, FinalHeight, FinalReach - 0.2f); // manually set in Inspector

        currentTargetIndex = 0;

        reactionTimes.Clear();
        targetIDs.Clear();
        targetPositions.Clear();
        enterPositions.Clear();
        exitPositions.Clear();
        trialStartTimes.Clear();
        timeEnter.Clear();
        timeExit.Clear();

        setUpEnvironment();
        displayTarget();
    }

    private void setUpEnvironment()
    {
        float A = testPairs[currentTestIndex].y;
        Vector3 scale;

        if (Mathf.Approximately(A, 0.35f))
            scale = new Vector3(0.14f, 0.0014f, 0.14f);
        else if (Mathf.Approximately(A, 0.45f))
            scale = new Vector3(0.10f, 0.001f, 0.10f);
        else if (Mathf.Approximately(A, 0.55f))
            scale = new Vector3(0.05f, 0.0005f, 0.05f);
        else
            scale = new Vector3(0.1f, 0.001f, 0.1f);

        for (int i = 0; i < numberOfCircles; i++)
        {
            float angle = 2 * Mathf.PI * i / numberOfCircles;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector3 pos = center + new Vector3(x, y, 0f);
            Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

            GameObject circle = Instantiate(circlePrefab, pos, rotation);
            circle.transform.localScale = scale;

            var rayScript = circle.GetComponent<ChangeColorOnTouch>();
            if (rayScript != null)
            {
                rayScript.id = i;
            }

            circles.Add(circle);
        }
    }

    public void RecordTouch(Vector3 enterPos, Vector3 exitPos, float enterTime, float exitTime)
    {
        int targetID = sequence[currentTargetIndex];
        targetIDs.Add(targetID);
        reactionTimes.Add(exitTime - enterTime);
        targetPositions.Add(circles[targetID].transform.position);
        enterPositions.Add(enterPos);
        exitPositions.Add(exitPos);
        trialStartTimes.Add(reactionStartTime);
        timeEnter.Add(enterTime);
        timeExit.Add(exitTime);
    }

    public void displayTarget()
    {
        if (currentTargetIndex < sequence.Count)
        {
            int targetID = sequence[currentTargetIndex];
            circles[targetID].GetComponent<Renderer>().material.color = newColor;
            reactionStartTime = Time.time;
        }
    }

    public void AdvanceToNextTarget()
    {
        int prevID = sequence[currentTargetIndex];
        circles[prevID].GetComponent<Renderer>().material.color = Color.white;

        currentTargetIndex++;

        if (currentTargetIndex < sequence.Count)
        {
            int nextID = sequence[currentTargetIndex];
            circles[nextID].GetComponent<Renderer>().material.color = newColor;
            reactionStartTime = Time.time;
        }
        else
        {
            SaveResultsToCSV();
            DestroyAllCircles();
            currentTestIndex++;
            StartNextTest();
        }
    }

    private void SaveResultsToCSV()
    {
        string path = Path.Combine(Application.dataPath, "FittsLawResults.csv");
        bool fileExists = File.Exists(path);

        using (StreamWriter writer = new StreamWriter(path, true))
        {
            if (!fileExists)
            {
                writer.WriteLine("ParticipantID,Condition,Block,No.,TargetID,TargetNumber,W,A,TargetX,TargetY,TargetZ,EnterX,EnterY,EnterZ,ExitX,ExitY,ExitZ,TimeTrialStart,TimeEnterTarget,TimeExitTarget");
            }

            for (int i = 0; i < targetIDs.Count; i++)
            {
                writer.WriteLine($"P,ConditionA,{currentBlock},{currentTestIndex + 1},{targetIDs[i]},{i},{radius:F2},{testPairs[currentTestIndex].y:F2}," +
                    $"{targetPositions[i].x:F3},{targetPositions[i].y:F3},{targetPositions[i].z:F3}," +
                    $"{enterPositions[i].x:F3},{enterPositions[i].y:F3},{enterPositions[i].z:F3}," +
                    $"{exitPositions[i].x:F3},{exitPositions[i].y:F3},{exitPositions[i].z:F3}," +
                    $"{trialStartTimes[i]:F3},{timeEnter[i]:F3},{timeExit[i]:F3}");
            }
        }

        Debug.Log("Results saved to CSV for block " + (currentBlock));
    }

    private void DestroyAllCircles()
    {
        foreach (GameObject circle in circles)
        {
            Destroy(circle);
        }

        circles.Clear();
    }
}
