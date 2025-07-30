using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDialog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Screen.width < Screen.height && Screen.width < 1000)
            ;
        else
            gameObject.SetActive(false);
    }

}
