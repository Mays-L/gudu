using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PositionFactory
{
    #region Fields
    protected System.Random _Random;

    /// <summary>
    /// Space between clouds
    /// </summary>
    protected readonly int GapSize = 0;

    protected RectTransform gamePanel;

    protected Vector3 leftButtom, leftUp, rightButtom, rightUp;
    protected Vector3 leftButtomVP, leftUpVP, rightButtomVP, rightUpVP;
    protected Vector3 center;

    protected float width, height;
    #endregion

    #region Methods

    protected void setGamePlayParameters()
    {
        gamePanel = GameObject.Find("GamePanel").GetComponent<RectTransform>();

        Vector3[] gamePlayCorners = new Vector3[4];
        gamePanel.GetWorldCorners(gamePlayCorners);
        leftButtom = gamePlayCorners[0];
        leftUp = gamePlayCorners[1];
        rightUp = gamePlayCorners[2];
        rightButtom = gamePlayCorners[3];

        width = rightUp.x - leftUp.x;
        height = rightUp.y - rightButtom.y;

        leftButtomVP = CameraController.Instance.camera.WorldToViewportPoint(leftButtom);
        leftUpVP = CameraController.Instance.camera.WorldToViewportPoint(leftUp);
        rightUpVP = CameraController.Instance.camera.WorldToViewportPoint(rightUp);
        rightButtomVP = CameraController.Instance.camera.WorldToViewportPoint(rightButtom);

        center = new Vector3((leftButtom.x + rightButtom.x) / 2, (leftUp.y + leftButtom.y) / 2, 0);
    }

    protected void SetScreenParametersAfterChangingSizeOfCamera()
    {
        leftButtom = CameraController.Instance.camera.ViewportToWorldPoint(leftButtomVP);
        leftUp = CameraController.Instance.camera.ViewportToWorldPoint(leftUpVP);
        rightUp = CameraController.Instance.camera.ViewportToWorldPoint(rightUpVP);
        rightButtom = CameraController.Instance.camera.ViewportToWorldPoint(rightButtomVP);
    }

  
    #endregion
}
