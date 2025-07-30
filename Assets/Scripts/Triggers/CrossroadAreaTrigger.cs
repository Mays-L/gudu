using System;
using UnityEngine;
using UnityEngine.Events;

// Triggers the dialog when player reaches the center of a crossroad
public class CrossroadAreaTrigger : MonoBehaviour
{
    [SerializeField]
    private int crossroadIndex;  // which crossroad (1-4)

    private bool hasTriggered = false;
    [SerializeField] private CrossroadPathTrigger[] branchColliders;



    private void Awake()
    {
        foreach (var branch in branchColliders)
        {
            branch.baseArea = this ;
        }


    }
    public void CrossRoadFinish()
    {
        foreach (var branch in branchColliders)
        {
            if (!( branch.isCameFrom|| branch.isCorrectPath))
                branch.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("entered hasTriggerd: " + hasTriggered);
        if (!hasTriggered && other.CompareTag("Player"))
        {
            Debug.Log("in player");
            hasTriggered = true;
            for (int i = 0; i < branchColliders.Length; i++)
            {
                Debug.Log("in for ");
                branchColliders[i].gameObject.SetActive(true);
                if (branchColliders[i].isCameFrom)
                    branchColliders[i].GetComponent<BoxCollider2D>().isTrigger = false;


            }
            foreach (var branch in branchColliders)
            {
                branch.gameObject.SetActive(true);
            }
            //Player.Instance.CanMove = false;
            Debug.Log("dialouge");
            MainGameManager.Instance.EnterCrossroad(crossroadIndex);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        Debug.Log("Exited and hasTrigger is : "+ hasTriggered);
        if (hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = false;
            Debug.Log("in if ...");
        }
    }
}
