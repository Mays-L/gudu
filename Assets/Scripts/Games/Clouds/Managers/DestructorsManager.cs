using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructorsManager : Singleton<DestructorsManager>
{
    #region Fields

    /// <summary>
    /// Flappy bird destructor
    /// </summary>
    [SerializeField]
    private GameObject FlappyBird;

    /// <summary>
    /// Static balloon destructor
    /// </summary>
    [SerializeField]
    private GameObject Balloon;

    /// <summary>
    /// Moving balloons destructor
    /// </summary>
    [SerializeField]
    private GameObject MovingBalloon;

    /// <summary>
    /// Thunder destructor
    /// </summary>
    [SerializeField]
    private GameObject Thunder;

    #endregion

    #region Methods

    /// <summary>
    /// Unity awake method
    /// </summary>
    void Awake()
    {
        base.Awake();
        SetAllDeactive();
    }

    /// <summary>
    /// Set destructor base of destructor type
    /// </summary>
    /// <param name="type">Type of destructor</param>
    public void SetDestructor(DistractorType type)
    {
        SetAllDeactive();

        if (type == DistractorType.Static)
        {
            Balloon.SetActive(true);
        }
        if (type == DistractorType.Dynamic)
        {
            System.Random r = new System.Random();
            int random = r.Next(0, 2);
            if (random == 1)
                MovingBalloon.SetActive(true);
            else
                FlappyBird.SetActive(true);
        }
        if (type == DistractorType.Hard)
        {
            Thunder.SetActive(true);
        }
    }

    /// <summary>
    /// Set all destructor deactive
    /// </summary>
    public void SetAllDeactive()
    {
        Balloon.SetActive(false);
        MovingBalloon.SetActive(false);
        FlappyBird.SetActive(false);
        Thunder.SetActive(false);
    }

    #endregion
}
