using UnityEngine;

// At the dead-end of a wrong path: teleport player back to center
public class PathEndTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform returnPoint;  // the center-of-crossroad transform

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = returnPoint.position;
            Player.Instance.CanMove = true;
        }
    }
}
