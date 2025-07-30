using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorOnMouseEnter : MonoBehaviour
{
    public Color onMouseEnterColor;

    public GameObject image;
    Image imageComponent;
    public void Awake()
    {
        if (image != null)
        imageComponent = image.GetComponent<Image>();
    }
    private void OnMouseEnter()
    {
        if (image != null)
        imageComponent.color = onMouseEnterColor;
    }
    private void OnMouseExit()
    {
        if(image != null)
        imageComponent.color = Color.white;
    }
}
