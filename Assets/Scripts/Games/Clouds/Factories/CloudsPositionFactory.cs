using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Position factory
/// </summary>
class CloudsPositionFactory : PositionFactory
{
    #region Fields

    private static System.Random _Random;

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

    #endregion

    #region Methods

    /// <summary>
    /// Unity Awake Method
    /// </summary>
    public void Awake()
    {
        if (_Random == null)
            _Random = new System.Random();
        SetScreenWidthHeight();

    }

    /// <summary>
    /// Generate random position in the screen
    /// </summary>
    /// <returns>Position 3d Vector</returns>
    public Vector3 RandomPosition()
    {
        SetScreenWidthHeight();
        int targetx = _Random.Next(-ScreenWidth / 2, ScreenWidth / 2);
        int targety = _Random.Next(-ScreenHeight / 2, ScreenHeight / 2);

        return new Vector3(targetx, targety, 0);
    }

    private void SetScreenWidthHeight()
    {
        try
        {
            canvas = GameObject.Find("GamePanel").GetComponent<RectTransform>();
            if (canvas is null)
                return;
            ScreenHeight = (int)canvas.rect.height;
            ScreenWidth = (int)canvas.rect.width;
            if (_Random == null)
                _Random = new System.Random();
        }
        catch  { }
    }

    /// <summary>
    /// Retun new position in movement and change target if needed
    /// </summary>
    /// <param name="position">current position</param>
    /// <param name="target">target position</param>
    /// <param name="speed">speed of movement</param>
    /// <returns></returns>
    public Vector3 Movement(Vector3 position, ref Vector3 target, double speed)
    {

        float step = (float)speed * Time.deltaTime;

        if (Math.Abs(position.x - target.x) < step
               || Math.Abs(position.y - target.y) < step)
        {
            Vector3 newPos = RandomPosition();
            newPos.x = Math.Sign(position.x) * Math.Abs(newPos.x);
            newPos.y = Math.Sign(position.y) * Math.Abs(newPos.y);
            target = newPos;
        }

        return Vector3.MoveTowards(position, target, step);

    }

    /// <summary>
    /// Get initial positions of clouds in a rectangle base of landscape or portrait screen
    /// </summary>
    /// <param name="number">Number of clouds</param>
    /// <param name="cloudSize">Size of clouds</param>
    /// <returns>List of positions vector</returns>
    public List<Vector3> GetCloudsInitialPosition(int number, int cloudSize)
    {
        if (canvas is null)
            SetScreenWidthHeight();
        bool landscape = ScreenWidth > ScreenHeight;
        List<Vector3> positions = new List<Vector3>();


        //Get width and height of cloud's triangle 
        int widthClouds = (int)Math.Sqrt(number);
        int rectWidth = (widthClouds - 1) * (GapSize + cloudSize);
        int heightClouds = (int)Math.Ceiling((double)number / widthClouds);
        int rectHeight = (heightClouds - 1) * (GapSize + cloudSize);

        if (landscape)
        {
            int temp = rectWidth;
            rectWidth = rectHeight;
            rectHeight = temp;
        }
        //Generate position for every cloud
        for (int i = 0; i < number; i++)
        {
            int x = i % widthClouds;
            int y = i / widthClouds;
            if (landscape)
            {
                int temp = x;
                x = y;
                y = temp;
            }
            Vector3 position = new Vector3(x * (cloudSize + GapSize) - rectWidth / 2, y * (cloudSize + GapSize) - rectHeight / 2, 0);
            positions.Add(position);
        }

        return positions;
    }

    #endregion
}

