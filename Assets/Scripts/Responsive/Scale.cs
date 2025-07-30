using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{

    Vector3[] corners;
    Vector3 leftBottom, leftUp, rightUp, rightBottom;
    public float width, height;
    void OnEnable()
    {
        corners = new Vector3[4];
       
        RectTransform rectTransformComponent = gameObject.GetComponent<RectTransform>();

        rectTransformComponent.GetWorldCorners(corners);
        leftBottom = CameraController.Instance.camera.WorldToScreenPoint(corners[0]);
        leftUp = CameraController.Instance.camera.WorldToScreenPoint(corners[1]);
        rightUp = CameraController.Instance.camera.WorldToScreenPoint(corners[2]);
        rightBottom = CameraController.Instance.camera.WorldToScreenPoint(corners[3]);


        float rectHeight = leftUp.y - leftBottom.y;
        float rectWidth = rightUp.x - leftUp.x;

        float newHeight = rectWidth * (height / width);
        Vector3 point1 = CameraController.Instance.camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 point2 = CameraController.Instance.camera.ScreenToWorldPoint(new Vector3(0, rectHeight - newHeight, 0));
        float distance = Vector3.Distance(point2, point1);
        float newY = transform.position.y + distance;
        Vector3 newPos = new Vector3(transform.position.x, newY, transform.position.z);
        transform.position = newPos;
        rectTransformComponent.sizeDelta= new Vector2(rectTransformComponent.sizeDelta.x, newHeight);
    }
}
