using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ResponsiveImage : MonoBehaviour
{
    public Sprite PortraitImage;
    public Sprite LandscapeImage;
    public Image image;

    /// <summary>
    /// Unity update method
    /// </summary>
    void Update()
    {
        if (Screen.width > Screen.height)
        {
            image.sprite = LandscapeImage;
        }
        else
        {
            image.sprite = PortraitImage;
        }
    }
}
