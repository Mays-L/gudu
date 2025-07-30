
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestsManager : Singleton<NestsManager>
{
    // The nests that is currently shown in the game
    List<GameObject> nests;
    [SerializeField]
    GameObject temporaryNest;

    public HoneyMemoryCodingFactory hiddenDataEncoder = new HoneyMemoryCodingFactory();

    void Start()
    {
        nests = new List<GameObject>();
    }
    /// <summary>
    /// Showing the nest on the screen
    /// </summary>
    /// <param name="NumberOfNests">The number of nest to show</param>
    public void ShowNests(int numberOfNests, int numberOfDistinctNests, int numberOfTargetedAreas)
    {
        List<Vector3> allPosition = new List<Vector3>();
        List<Vector3> targetedPosition = new List<Vector3>();
        CalculatePositions(numberOfNests, numberOfDistinctNests, numberOfTargetedAreas, ref targetedPosition, ref allPosition);
        SpawnNests(allPosition);
        MakeDistinct(allPosition,targetedPosition);
    }

    /// <summary>
    /// Calculating the positions for spawning nests
    /// </summary>
    /// <param name="nameOfNest"></param>
    /// <param name="numberOfNests"></param>
    /// <returns></returns>
    void CalculatePositions(int numberOfNests, int numberOfDistinctNests, int numberOfTargetedAreas, ref List<Vector3> targetedPositions, ref List<Vector3> positions)
    {
        RectTransform nestRectTransform = (RectTransform)temporaryNest.transform;
        float nestWidth = nestRectTransform.rect.width*nestRectTransform.transform.localScale.x ;
        float nestHeight = nestRectTransform.rect.height * nestRectTransform.transform.localScale.y;
        HoneyMemoryPositionFactory HMPF = new HoneyMemoryPositionFactory();
        HMPF.GetNestsInitialPosition(numberOfNests, numberOfDistinctNests, nestWidth, nestHeight, nestWidth * 0.05f, numberOfTargetedAreas, ref targetedPositions, ref positions);
    }

    /// <summary>
    /// Spawning nests on the game play
    /// </summary>
    private void SpawnNests(List<Vector3> positions)
    {
        GameObject parent = GameObject.Find("GamePanel");
        for (int i = 0; i < positions.Count; i++)
        {
            GameObject newNest = Instantiate(temporaryNest, positions[i], Quaternion.identity,parent.transform);
            newNest.transform.localPosition = positions[i];
            newNest.GetComponent<Nest>().myIndex = i;
            nests.Add(newNest);
        }
    }

    /// <summary>
    /// Removing all the sets on the screen
    /// </summary>
    public void RemoveAllNests()
    {
        while (nests.Count > 0)
        {
            GameObject n = nests[nests.Count - 1];
            nests.RemoveAt(nests.Count - 1);
            Destroy(n);

        }
    }

    public void DisableSelecting()
    {
        foreach(GameObject nest in nests)
        {
            nest.GetComponent<Nest>().IsChosen = true;
        }
    }


    public void ShowDistinct(float time)
    {
        EventManager.Instance.InvokeEvent("StartingDistinctionEvent");
        Timers.Instance.StartTimer(time, "EndingDistinctionEvent");
    }
    public void MakeDistinct(List<Vector3> AllPosition, List<Vector3> TargetedPosition)
    {
        //List<int> chosenIndices = getIndices(numberOfDistinctNests, nests.Count);

        for (int i=0;i<AllPosition.Count;i++)
        {
            if (TargetedPosition.Contains(AllPosition[i]))
            {
                nests[i].GetComponent<Nest>().IsDistinct = true;
                hiddenDataEncoder.nestsType[i] = 1;
            }
        }
    }

    List<int> getIndices(int numberOfIndices, int numberOfAll)
    {
        List<int> usedIndices = new List<int>();
        for (int i = 0; i < numberOfIndices; i++) 
        {
            int newIndex = Random.Range(0, numberOfAll);
            while (usedIndices.Contains(newIndex))
                newIndex = Random.Range(0, numberOfAll);
            usedIndices.Add(newIndex);
        }
        return usedIndices;
    }

    internal Vector3 GetTheAnswerPosition()
    {
        foreach (GameObject nest in nests)
        {
            Nest n = nest.GetComponent<Nest>();
            if (n.IsDistinct == true && !n.IsChosen)
                if (!n._startingState)
                    return n.GetComponent<RectTransform>().localPosition;
                else
                    return new Vector3(0,0,10);
        }
        return new Vector3(0, 0, 10);
    }
        
}
