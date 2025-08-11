using UnityEngine;
using System.Collections.Generic;


public static class Util
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count); // Unity's Random
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
