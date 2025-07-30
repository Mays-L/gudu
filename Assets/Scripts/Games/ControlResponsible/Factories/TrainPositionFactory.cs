using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainPositionFactory
{
    /// <summary>
    /// Height of screen
    /// </summary>
    public int ScreenHeight;

    /// <summary>
    /// Width of screen
    /// </summary>
    public int ScreenWidth;

    /// <summary>
    /// Space between clouds
    /// </summary>
    private readonly int GapSize = 0;

    private RectTransform canvas;

    private void SetScreenWidthHeight()
    {
        try
        {
            canvas = GameObject.Find("GamePanel").GetComponent<RectTransform>();
            if (canvas is null)
                return;
            ScreenHeight = (int)canvas.rect.height;
            ScreenWidth = (int)canvas.rect.width;
        }
        catch { }
    }
    internal List<Vector3> GetInitialPositions(int count, GameObject wagon)
    {
        float wagonWidth = wagon.transform.localScale.x * wagon.GetComponent<RectTransform>().rect.width;
        float wagonHeight = wagon.GetComponent<RectTransform>().rect.height;
        float rectWidth = wagonWidth * count;
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            Vector3 position = new Vector3(i * (wagonWidth + GapSize) - rectWidth / 2 + wagonWidth/2, wagonHeight/2, 0);
            positions.Add(position);
        }

        SetScreenWidthHeight();
        bool portrait = Screen.width < Screen.height;
        if (portrait)
        {
            if (count == 4)
            {
                positions = GetInitialPositions(3, wagon);
                positions.Add(new Vector3(0, 2*wagonHeight, 0));
            }
            if (count == 5)
            {
                positions = GetInitialPositions(3, wagon);
                positions.Add(new Vector3(-wagonWidth/2, 2*wagonHeight, 0));
                positions.Add(new Vector3(wagonWidth / 2, 2*wagonHeight, 0));
            }
        }
        return positions;
    }

    internal Vector3 GetTrainInitialPosition(GameObject wagon)
    {
        SetScreenWidthHeight();
        return new Vector3(ScreenWidth,-ScreenHeight/4, 0);
    }

    internal Vector3 GetNewWagonPosition(GameObject lastWagon)
    {
        float wagonWidth = lastWagon.transform.localScale.x * lastWagon.GetComponent<RectTransform>().rect.width;
        return lastWagon.transform.localPosition + new Vector3(wagonWidth,0,0);
    }
}
