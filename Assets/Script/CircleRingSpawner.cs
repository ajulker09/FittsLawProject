using UnityEngine;
using System;
using System.Collections;
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

    public GameObject circlePrefab;
    public GameObject Spawner;

    // new 08-27
    public GameObject dummyPlane;

    public GameObject rightCursorVisual;
    public GameObject leftCursorVisual;
    private GameObject cursorVisual;

    public int numberOfCircles = 10;

    private Color newColor = Color.green;
    private Color defaultColor = new Color(32f / 255f, 150f / 255f, 243f / 255f, 1f);
    private Color touchColor = Color.white;

    public static int currentTargetIndex = 0;
    public static List<int> sequence;

    private float reactionStartTime;
    private List<GameObject> circles = new List<GameObject>();

    private List<float> AMPLITUDES = new List<float> { 0.30f, 0.45f, 0.6f };
    private List<float> WIDTHS = new List<float> { 0.025f, 0.05f, 0.1f };

    //private List<float> AMPLITUDES = new List<float> { 0.30f };
    //private List<float> WIDTHS = new List<float> { 0.03f };

    private List<Vector2> taskPairs;
    private int currentSequenceIndex = 0;

    public Vector3 center;
    public GameObject leftHand;
    public GameObject rightHand;
    private GameObject hand;

    private List<int> targetIDs = new List<int>();
    private List<float> reactionTimes = new List<float>();
    private List<Vector3> targetPositions = new List<Vector3>();
    private List<float> trialStartTimes = new List<float>();
    private List<float> timeEnter = new List<float>();

    // new 08-27
    private List<float> timeSelect = new List<float>();
    private List<Vector3> selectPositions = new List<Vector3>();
    private List<int> selectAttempts = new List<int>();
    
    // new 08-27
    // Number of select attempts
    private int numSelectAttempts = 1; 

    void OnEnable()
    {
        manager = experiment.GetComponent<ExperimentManager>();

        if (manager.IsLeftHand())
        {
            rightHand.SetActive(false);
            cursorVisual = rightCursorVisual;
        }
        else
        {
            leftHand.SetActive(false);
            cursorVisual = leftCursorVisual;
        }

        taskPairs = SetupTaskPairs();
        sequence = GenerateAcrossSequence(numberOfCircles);

        if (blockFinishedUI != null) blockFinishedUI.SetActive(false);
        if (conditionFinishedUI != null) conditionFinishedUI.SetActive(false);

        ClearInteractorState();

        StartNextSequence();
    }

    public string getCondition()
    {
        ExperimentManager.Condition condition = manager.GetCondition();
        return condition.ToString();
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

            DestroyAllCircles();
            ClearInteractorState();

            if (manager.GetBlock() < manager.NUM_BLOCKS)
            {
                // new 08-27
                dummyPlane.SetActive(false);


                var gizmo = leftHand != null ? leftHand.GetComponent("RayInteractorDebugGizmos") : null;
                if (gizmo != null) ((MonoBehaviour)gizmo).enabled = false;
                var gizmo1 = rightHand != null ? rightHand.GetComponent("RayInteractorDebugGizmos") : null;
                if (gizmo1 != null) ((MonoBehaviour)gizmo1).enabled = false;

                if (blockFinishedUI != null) blockFinishedUI.SetActive(true);
                gameObject.SetActive(false);
                return;
            }
            else
            {

                // new 08-27
                dummyPlane.SetActive(false);

                Debug.Log("Condition " + manager.GetCondition() + " completed.");
                manager.SetBlock(0);
                manager.IncrementCondition();

                DestroyAllCircles();
                ClearInteractorState();

                var gizmo = leftHand != null ? leftHand.GetComponent("RayInteractorDebugGizmos") : null;
                if (gizmo != null) ((MonoBehaviour)gizmo).enabled = false;
                var gizmo1 = rightHand != null ? rightHand.GetComponent("RayInteractorDebugGizmos") : null;
                if (gizmo1 != null) ((MonoBehaviour)gizmo1).enabled = false;

                if (conditionFinishedUI != null) conditionFinishedUI.SetActive(true);
                gameObject.SetActive(false);
                return;
            }
        }
        // new 08-27
        dummyPlane.SetActive(true);

        center = new Vector3(0f, 1.4f, 0.1f);
    
      

        currentTargetIndex = 0;
        reactionTimes.Clear();
        targetIDs.Clear();
        targetPositions.Clear();
        

        trialStartTimes.Clear();
        timeEnter.Clear();

        // new 08-27
        timeSelect.Clear();
        selectAttempts.Clear();
        selectPositions.Clear();

        DestroyAllCircles();
        ClearInteractorState();

        SetUpEnvironment();

        
        StartCoroutine(ResetRaysNextFrame());

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
            circle.transform.localScale = new Vector3(W, 0.001f, W);

            var col = circle.GetComponent<Collider>();
            if (col == null) col = circle.AddComponent<SphereCollider>();
            col.enabled = true;

            ChangeColorOnTouch script = circle.GetComponent<ChangeColorOnTouch>();
            script.id = i;

            var wrapper = circle.GetComponent<Oculus.Interaction.InteractableUnityEventWrapper>();
            if (wrapper != null)
            {
                wrapper.WhenHover.RemoveAllListeners();
                wrapper.WhenUnhover.RemoveAllListeners();
                wrapper.WhenSelect.RemoveAllListeners();
                wrapper.WhenUnselect.RemoveAllListeners();

                wrapper.WhenHover.AddListener(script.OnHover);
                wrapper.WhenUnhover.AddListener(script.OnUnhover);
                wrapper.WhenSelect.AddListener(script.OnSelect);
                wrapper.WhenUnselect.AddListener(script.OnUnselect);
            }

            circles.Add(circle);
        }

        // new 08-27
        Vector3 dummyPos = center;
        dummyPos.z += 0.01f;
        dummyPlane.transform.position = dummyPos;
        var handler = dummyPlane.GetComponent<DummyPlaneHandler>();
        // var dummyWrapper = dummyPlane.GetComponent<Oculus.Interaction.InteractableUnityEventWrapper>();
        // if (dummyWrapper != null)
        // {
        //     dummyWrapper.WhenSelect.RemoveAllListeners();
        //     dummyWrapper.WhenUnselect.RemoveAllListeners();

        //     dummyWrapper.WhenSelect.AddListener(handler.OnUnselectEvent);
        //     dummyWrapper.WhenUnselect.AddListener(handler.OnSelectEvent);
        // }
    }

    // new 08-27
    public void IncrementSelectAttempts()
    {
        numSelectAttempts++;
    }

    public int GetSelectAttempts() {
        return numSelectAttempts;
    }

    // new 08-27, don't need enterPos/exitPos
    public void RecordTouch(float enterTime, float selectTime)
    {
        int targetID = sequence[currentTargetIndex];
        targetIDs.Add(targetID);
        reactionTimes.Add(selectTime - reactionStartTime);
        targetPositions.Add(circles[targetID].transform.position);

        trialStartTimes.Add(reactionStartTime);
        timeEnter.Add(enterTime);
        timeSelect.Add(selectTime);
        // new 08-27
        selectAttempts.Add(numSelectAttempts);
        selectPositions.Add(cursorVisual.transform.position);
    }

    public void DisplayTarget()
    {
        int targetID = sequence[currentTargetIndex];

        var script = circles[targetID].GetComponent<ChangeColorOnTouch>();
        if (script != null) script.SetOriginalColor(newColor);

        Vector3 posTarget = circles[targetID].transform.position;
        posTarget.z = center.z - 0.0001f;
        circles[targetID].transform.position = posTarget;

        circles[targetID].GetComponent<Renderer>().material.color = newColor;

        reactionStartTime = Time.time;

        // new 08-27
        numSelectAttempts = 1;
    }

    public void AdvanceToNextTarget()
    {
        int prevID = sequence[currentTargetIndex];

        Vector3 posPrev = circles[prevID].transform.position;
        posPrev.z = center.z;
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

            ClearInteractorState();

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
             
                writer.WriteLine("ParticipantID,Condition,Block,No.,TargetID,TargetNumber,W,A,TargetX,TargetY,TargetZ,SelectX,SelectY,SelectZ,TimeTrialStart,TimeEnterTarget,TimeSelect,TotalReactionTime,SelectAttempts");
            }

            for (int i = 0; i < targetIDs.Count; i++)
            {
                writer.WriteLine(
                    $"{manager.GetPID()},{manager.GetCondition()},{manager.GetBlock()},{currentSequenceIndex + 1},{targetIDs[i]},{i},{taskPairs[currentSequenceIndex].x:F2},{taskPairs[currentSequenceIndex].y:F2}," +
                    $"{targetPositions[i].x:F3},{targetPositions[i].y:F3},{targetPositions[i].z:F3}," +
                    // new 08-27
                    $"{selectPositions[i].x:F3},{selectPositions[i].y:F3},{selectPositions[i].z:F3}," +
                    $"{trialStartTimes[i]:F3},{timeEnter[i]:F3},{timeSelect[i]:F3},{reactionTimes[i]:F3}," +
                    // new 08-27
                    $"{selectAttempts[i]}"
                );
            }
        }

        Debug.Log("Results saved to CSV for block " + (manager.GetBlock()));
    }

    private void DestroyAllCircles()
    {
        foreach (GameObject circle in circles)
        {
            if (circle != null) Destroy(circle);
        }
        circles.Clear();
    }

    public void EnterFeedback(int circleID)
    {
        ExperimentManager.Condition condition = manager.GetCondition();

        if (condition == ExperimentManager.Condition.NonDominant || condition == ExperimentManager.Condition.Dominant)
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

        // If needed:
        // if (condition == ExperimentManager.Condition.NonDominant || condition == ExperimentManager.Condition.Dominant)
        // {
        //     networking.GetComponent<Server>().SendMessageToClient("Exit " + circleID);
        // }
    }

    private void ClearInteractorState()
    {
        if (leftHand != null)
        {
            var rays = leftHand.GetComponentsInChildren<Oculus.Interaction.RayInteractor>(true);
            foreach (var r in rays) { r.enabled = false; r.enabled = true; }
        }
        if (rightHand != null)
        {
            var rays = rightHand.GetComponentsInChildren<Oculus.Interaction.RayInteractor>(true);
            foreach (var r in rays) { r.enabled = false; r.enabled = true; }
        }
    }

    
    private IEnumerator ResetRaysNextFrame()
    {
        
        yield return null;

        if (leftHand != null)
        {
            leftHand.SetActive(false);
            leftHand.SetActive(true);
        }

        if (rightHand != null)
        {
            rightHand.SetActive(false);
            rightHand.SetActive(true);
        }
    }
}
