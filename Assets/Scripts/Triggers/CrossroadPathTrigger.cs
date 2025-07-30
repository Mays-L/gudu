using System;
using UnityEngine;

// Attached to each path entrance collider.
// Records the chosen direction as soon as the player enters that path.
public class CrossroadPathTrigger : MonoBehaviour
{
    [SerializeField]
    private int crossroadIndex;      // same index as the AreaTrigger

    [SerializeField]
    private Direction direction;     // Up, Down, Left or Right

    [SerializeField]
    public bool isCorrectPath;      // is this the correct branch?
    [SerializeField]
    public bool isCameFrom;
    private bool hasTriggered = false;

    [HideInInspector]
    public CrossroadAreaTrigger baseArea;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            MainGameManager.Instance.RecordDirection(
                crossroadIndex, direction, isCorrectPath
            );
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isCameFrom || isCorrectPath ) this.GetComponent<BoxCollider2D>().isTrigger = false;
        
        if (hasTriggered && other.CompareTag("Player"))
        {
            //hasTriggered = false;
            baseArea.CrossRoadFinish();
        }
    }
}
