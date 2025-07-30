using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResponsiveObject : MonoBehaviour
{
    public GameObject landScapeObject;
    public GameObject portraitObject;
    void Update()
    {
        if (Screen.width > Screen.height)
        {
            landScapeObject.SetActive(true);
            portraitObject.SetActive(false);
        }
        else
        {
            landScapeObject.SetActive(false);
            portraitObject.SetActive(true);
        }

    }
    public GameObject GetActiveObject()
    {
        if (Screen.width > Screen.height)
        {
            return landScapeObject;
        }
        else
        {
            return portraitObject;
        }
    }
}
