
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomGenerator
{
    /// <summary>
    /// Generate a random list between 0 to range without repeat
    /// </summary>
    /// <param name="length">length of returned array</param>
    /// <param name="range">range of array numbers</param>
    /// <returns>random integer list</returns>
    public static List<int> GetRandomList(int length, int range)
    {
        List<int> usedIndices = new List<int>();
        for (int i = 0; i < length; i++)
        {
            int newIndex = Random.Range(0, range);
            while (usedIndices.Contains(newIndex))
                newIndex = Random.Range(0, range);
            usedIndices.Add(newIndex);
        }
        return usedIndices;
    }

    /// <summary>
    /// Get random number
    /// </summary>
    /// <param name="range">Maximum of value</param>
    /// <returns>Random integer</returns>
    public static int GetRandomNumber(int range)
    {
        return Random.Range(0, range);
    }

    internal static bool GetRandomBool(int trueRate)
    {
        int number = Random.Range(0, trueRate);
        return number == 0;
    }

    internal static List<T> RandomSort<T>(List<T> notSortedList)
    {
        List<int> indexes = GetRandomList(notSortedList.Count, notSortedList.Count);
        List<T> sortedList = new List<T>();
        foreach(int index in indexes)
        {
            sortedList.Add(notSortedList[index]);
        }
        return sortedList;
    }
}
