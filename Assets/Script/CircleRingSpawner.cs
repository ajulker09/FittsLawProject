using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;


public class CircleRingSpawner : MonoBehaviour
{

    // Networking object, exclusively for the watch 
    public GameObject networking;

    // A game object to hold experiment manager
    public GameObject experiment;

    // This holds any information related to the experiment
    private ExperimentManager manager;

    // UI Stuff
    public GameObject blockFinishedUI;
    public GameObject conditionFinishedUI;
    public GameObject handRayInteractor;



    public GameObject circlePrefab;
    public GameObject Spawner;

    public int numberOfCircles = 10;

    private Color newColor = Color.green;
    private Color defaultColor = new Color(32f / 255f, 150f / 255f, 243f / 255f, 1f);
    private Color touchColor = Color.white;

    

    public static int currentTargetIndex = 0;
    public static List<int> sequence; //= new List<int> { 0, 6, 12, 5, 11, 4, 10, 3, 9, 2, 8, 1, 7 };

    private float reactionStartTime;
    private List<GameObject> circles = new List<GameObject>();


    private List<float> AMPLITUDES = new List<float> { 0.6f }; //{ 0.3f, 0.45f };//,0.6f}; 
    private List<float> WIDTHS = new List<float> { 0.1f };//{ 0.025f, 0.05f, 0.1f };//, 0.15f};
    private List<Vector2> taskPairs;

    private int currentSequenceIndex = 0;

    public Vector3 center;
    public GameObject leftHand;
    public GameObject rightHand;
    private GameObject hand;

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
        manager = experiment.GetComponent<ExperimentManager>();

        if (manager.IsLeftHand())
        {
            rightHand.SetActive(false);
        }
        else
        {
            leftHand.SetActive(false);
        }

        taskPairs = SetupTaskPairs();
        sequence = GenerateAcrossSequence(numberOfCircles);

        StartNextSequence();
    }

    private List<Vector2> SetupTaskPairs()
    {
        List<Vector2> pairs = new List<Vector2>();
        foreach (float W in WIDTHS)
        {
            foreach (float A in AMPLITUDES)
            {
                pairs.Add(new Vector2(W, A));
            }
        }
        pairs.Shuffle();
        return pairs;
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

    private void StartNextSequence()
    {

        if (currentSequenceIndex >= taskPairs.Count)
        {
            currentSequenceIndex = 0;
            Debug.Log("Block " + manager.GetBlock() + " completed."); 
            manager.IncrementBlock();


            if (manager.GetBlock() < manager.NUM_BLOCKS)
            {
                var gizmo = handRayInteractor.GetComponent("RayInteractorDebugGizmos");
                if (gizmo != null)
                {
                    ((MonoBehaviour)gizmo).enabled = false;
                }
                blockFinishedUI.SetActive(true);
                gameObject.SetActive(false);
                return;
            } else {
                Debug.Log("Condition " + manager.GetCondition() + " completed.");

                manager.SetBlock(0);
                manager.IncrementCondition();

                var gizmo = handRayInteractor.GetComponent("RayInteractorDebugGizmos");
                if (gizmo != null)
                {
                    ((MonoBehaviour)gizmo).enabled = false;
                }

                conditionFinishedUI.SetActive(true);
                gameObject.SetActive(false);
                return;
            }
        }

        
        //where the circle will spawn in the game
        center = new Vector3(0f, 1.4f, 0.3f);


        currentTargetIndex = 0;

        reactionTimes.Clear();
        targetIDs.Clear();
        targetPositions.Clear();
        enterPositions.Clear();
        exitPositions.Clear();
        trialStartTimes.Clear();
        timeEnter.Clear();
        timeExit.Clear();

        SetUpEnvironment();
        DisplayTarget();
    }

    private void SetUpEnvironment()
    {
        float W = taskPairs[currentSequenceIndex].x;
        float A = taskPairs[currentSequenceIndex].y;


        for (int i = 0; i < numberOfCircles; i++)
        {
            float angle = 2 * Mathf.PI * i / numberOfCircles;
            float x = Mathf.Cos(angle) * A / 2;
            float y = Mathf.Sin(angle) * A / 2;

            Vector3 pos = center + new Vector3(x, y, 0f);
            Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

            GameObject circle = Instantiate(circlePrefab, pos, rotation);

            Vector3 currentScale = circle.transform.localScale;
            currentScale = new Vector3(W, 0.001f, W);
            circle.transform.localScale = currentScale;

            ChangeColorOnTouch script = circle.GetComponent<ChangeColorOnTouch>();

            script.id = i;
            //script.finger = indexFinger;

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

    public void DisplayTarget()
    {
        int targetID = sequence[currentTargetIndex];

        // Get current position
        Vector3 posTarget = circles[targetID].transform.position;

        // Change only the Z
        posTarget.z = center.z - 0.0001f;

        // Reassign modified position
        circles[targetID].transform.position = posTarget;

        circles[targetID].GetComponent<Renderer>().material.color = newColor;
        reactionStartTime = Time.time;

    }

    public void AdvanceToNextTarget()
    {
        int prevID = sequence[currentTargetIndex];

        // Get current position
        Vector3 posPrev = circles[prevID].transform.position;

        // Change only the Z
        posPrev.z = center.z;

        // Reassign modified position
        circles[prevID].transform.position = posPrev;
        circles[prevID].GetComponent<Renderer>().material.color = defaultColor;


        currentTargetIndex++;


        if (currentTargetIndex < sequence.Count)
        {
            DisplayTarget();
        }
        else
        {
            SaveResultsToCSV();
            DestroyAllCircles();
            currentSequenceIndex++;
            StartNextSequence();
        }
    }

    private void SaveResultsToCSV()
    {
        string path = Path.Combine(Application.persistentDataPath, "FittsLawResults.csv");
        bool fileExists = File.Exists(path);

        using (StreamWriter writer = new StreamWriter(path, true))
        {
            if (!fileExists)
            {
                writer.WriteLine("ParticipantID,Condition,Block,No.,TargetID,TargetNumber,W,A,TargetX,TargetY,TargetZ,EnterX,EnterY,EnterZ,ExitX,ExitY,ExitZ,TimeTrialStart,TimeEnterTarget,TimeExitTarget");
            }

            for (int i = 0; i < targetIDs.Count; i++)
            {
                writer.WriteLine($"{manager.GetPID()},{manager.GetCondition()},{manager.GetBlock()},{currentSequenceIndex + 1},{targetIDs[i]},{i},{taskPairs[currentSequenceIndex].x:F2},{taskPairs[currentSequenceIndex].y:F2}," +
                    $"{targetPositions[i].x:F3},{targetPositions[i].y:F3},{targetPositions[i].z:F3}," +
                    $"{enterPositions[i].x:F3},{enterPositions[i].y:F3},{enterPositions[i].z:F3}," +
                    $"{exitPositions[i].x:F3},{exitPositions[i].y:F3},{exitPositions[i].z:F3}," +
                    $"{trialStartTimes[i]:F3},{timeEnter[i]:F3},{timeExit[i]:F3}");
            }
        }

        Debug.Log("Results saved to CSV for block " + (manager.GetBlock()));
    }

    private void DestroyAllCircles()
    {
        foreach (GameObject circle in circles)
        {
            Destroy(circle);
        }

        circles.Clear();
    }



    public void EnterFeedback(int circleID)
    {
        ExperimentManager.Condition condition = manager.GetCondition();
        if (condition == ExperimentManager.Condition.NonDominant
            || condition == ExperimentManager.Condition.Dominant)
        {
            networking.GetComponent<Server>().SendMessageToClient("Enter " + circleID);

        }
        else if (condition == ExperimentManager.Condition.Visual)
        {
            circles[circleID].GetComponent<Renderer>().material.color = touchColor;
        }
    }

    public void ExitFeedback(int circleID)
    {
        circles[circleID].GetComponent<Renderer>().material.color = defaultColor;
        ExperimentManager.Condition condition = manager.GetCondition();
        // If we want feedback on exit, enable this 
        // if (condition == ExperimentManager.Condition.NonDominant 
        //     || condition == ExperimentManager.Condition.Dominant) {
        //     networking.GetComponent<Server>().SendMessageToClient("Exit " + circleID);

        // }    
    }
}