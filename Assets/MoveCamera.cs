using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Player movingState;
    public float offsetY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(movingState.transform.position.x, movingState.transform.position.y+ offsetY, transform.position.z);
    }
}
