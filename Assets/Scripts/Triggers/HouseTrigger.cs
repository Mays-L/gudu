using UnityEngine;
using UnityEngine.SceneManagement;

// When player reaches the correct house, finish the stage
public class HouseTrigger : MonoBehaviour
{
    public Transform[] pointers;
    public int sceneInt;
    public WaysToGo[] ways;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            other.GetComponent<Player>().MoveToHouse(pointers, ways,this);
        }
    }


    public void Finish()
    {
        SceneManager.LoadScene(sceneInt);
    }
}
