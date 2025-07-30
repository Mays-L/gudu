using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ResponsivePosition : MonoBehaviour
{
    public Vector2 PortraitPosition;
    public Vector2 LandscapePosition;
    /// <summary>
    /// Unity update method
    /// </summary>
    void Update()
    {
        if (Screen.width > Screen.height)
        {
            gameObject.transform.localPosition = LandscapePosition;
        }
        else
        {
            gameObject.transform.localPosition = PortraitPosition;
        }
    }
}
