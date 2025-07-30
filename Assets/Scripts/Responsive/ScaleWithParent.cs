using System;
using UnityEngine;

public class ScaleWithParent : MonoBehaviour
{

    Vector3[] corners;

    private void Start()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        RectTransform parentGameObject  = (RectTransform) parent.transform;

        corners = new Vector3[4];

        parentGameObject.GetLocalCorners(corners);
        float width = Math.Abs(corners[0].x - corners[3].x);
        float height = Math.Abs(corners[1].y - corners[0].y);
        transform.localScale = new Vector2(width, height);
    }
}
