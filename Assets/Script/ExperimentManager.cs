using UnityEngine;
using System.Collections.Generic;


public class ExperimentManager : MonoBehaviour
{
    public enum Condition
    {
        NonDominant,
        Dominant,
        Visual,
        None
    }

    public static readonly Condition[][] CONDITION_ORDERINGS = new Condition[][]{
        new Condition[]{ Condition.NonDominant, Condition.Dominant,   Condition.Visual, Condition.None },
        new Condition[]{ Condition.Dominant,    Condition.NonDominant, Condition.None,   Condition.Visual },
        new Condition[]{ Condition.Visual,      Condition.None,        Condition.Dominant, Condition.NonDominant },
        new Condition[]{ Condition.None,        Condition.Visual,      Condition.NonDominant, Condition.Dominant }

    };



    public int NUM_BLOCKS = 3;

    private int conditionIndex = 0;
    private int block = 0;

    private int pid = 0;
    private bool leftHand = false;

    private Condition[] conditionOrder;



    public void SetupParticipant(int mPid, bool mLeftHand)
    {
        pid = mPid;
        conditionOrder = CONDITION_ORDERINGS[pid % (CONDITION_ORDERINGS.Length)];
        leftHand = mLeftHand;
        Debug.Log("left Hand: "+leftHand);
    }

    public int GetPID()
    {
        return pid;
    }

    public int GetConditionIndex()
    {
        return conditionIndex;
    }

    public bool IsLeftHand()
    {
        return leftHand;
    }

    public void SetConditionIndex(int idx)
    {
        conditionIndex = idx;
    }

    public void IncrementCondition()
    {
        conditionIndex++;
    }

    public Condition GetCondition()
    {
        return conditionOrder[conditionIndex];
    }

    public int GetBlock()
    {
        return block;
    }

    public void SetBlock(int idx)
    {
        block = idx;
    }

    public void IncrementBlock()
    {
        ;
        block++;
    }

}
