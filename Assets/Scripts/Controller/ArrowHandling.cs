using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Arrows
{
    Left = 0,
    Down = 1,
    Right = 2,
    Up = 3
}
public class ArrowHandling : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        ArrowClickedHandling();
    }

    void ArrowClickedHandling()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpArrowClicked();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightArrowClicked();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DownArrowClicked();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftArrowClicked();
        }
    }


    public void UpArrowClicked()
    {
        EventManager.Instance.InvokeEvent("UpArrowClicked");
    }
    public void RightArrowClicked()
    {
        EventManager.Instance.InvokeEvent("RightArrowClicked");
    }
    public void DownArrowClicked()
    {
        EventManager.Instance.InvokeEvent("DownArrowClicked");
    }
    public void LeftArrowClicked()
    {
        EventManager.Instance.InvokeEvent("LeftArrowClicked");
    }
}
