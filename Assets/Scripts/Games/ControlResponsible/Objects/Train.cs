using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField]
    public float Speed { get; set; }

    public bool IsMoving { get; internal set; }

    void Update()
    {
        if(IsMoving)
        {
            float step = (float)Speed * Time.deltaTime;
            transform.position += new Vector3(-step-Time.deltaTime/2, 0  , 0);
        }
    }

    public void StopMoving()
    {
        IsMoving = false;
    } 
    public void StartMoving()
    {
        IsMoving = true;
    }
}
