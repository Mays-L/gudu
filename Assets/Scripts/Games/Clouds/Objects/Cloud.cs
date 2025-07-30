
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represent cloud script
/// </summary>
public class Cloud : MonoBehaviour
{
    #region Fields

    /// <summary>
    /// Type of cloud
    /// </summary>
    internal CloudType Type = CloudType.Normal;

    /// <summary>
    /// Target position for random movement
    /// </summary>
    [SerializeField]
    private Vector3 Target;

    /// <summary>
    /// Is moving or not
    /// </summary>
    private bool Moving = false;

    /// <summary>
    /// Cloud speed
    /// </summary>
    private double Speed = 1000;

    /// <summary>
    /// Slowing down acceleration
    /// </summary>
    private double Acceleration = 15;

    /// <summary>
    /// Is stoped or not
    /// </summary>
    public bool Stoped = false;

    /// <summary>
    /// Secound counter for cloud
    /// </summary>
    private double SecoundCount = 1;

    /// <summary>
    /// Index of cloud
    /// </summary>
    private int cloudIndex = 0;

    /// <summary>
    /// Cloud is clicked or not
    /// </summary>
    internal bool Clicked = false;

    private CloudsPositionFactory _PositionFactory;

    #endregion

    #region Methods

    /// <summary>
    /// Unity awake method
    /// </summary>
    void Awake()
    {
        if (_PositionFactory == null)
            _PositionFactory = new CloudsPositionFactory();
    }

    /// <summary>
    /// Unity start method
    /// </summary>
    void Start()
    {
        SetSprite();
        StartCoroutine(EnableMoving());
    }

    /// <summary>
    /// Unity update method
    /// </summary>
    void Update()
    {
        if (Moving)
        {
            transform.localPosition = _PositionFactory.Movement(transform.localPosition, ref Target, Speed);
            SecoundCount -= Time.deltaTime;

            //after each second
            if (SecoundCount <= 0)
            {
                Speed -= Acceleration;
                if (Speed <= 0)
                {
                    Moving = false;
                    Stoped = true;
                    AudioManager.Instance.PlayStopClip();
                    DisableAnimation();

                    Timers.Instance.StartTimer("ResponseTimer", 0);
                    Timers.Instance.StartAnswerTimer();
                }
                SecoundCount = 1;
            }

        }

    }

    /// <summary>
    /// Detect collisions between the GameObjects with Colliders attached
    /// If cloud has collision the target have been changed
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cloud")
        {
            Vector3 newTarget = _PositionFactory.RandomPosition();
            int sx = Math.Sign(Target.x) > 0 ? 1 : -1;
            int sy = Math.Sign(Target.y) > 0 ? 1 : -1;
            Target = new Vector3(Math.Abs(newTarget.x) * sx, Math.Abs(newTarget.y) * sy, 0);
        }
    }

    /// <summary>
    /// Returns the index of the target area
    /// </summary>
    public int getTargetArea () 
    {
        int sx = Math.Sign(Target.x) > 0 ? 1 : -1;
        int sy = Math.Sign(Target.y) > 0 ? 1 : -1;
        if (sx > 0 && sy > 0)
            return 0;
        else if (sx < 0 && sy > 0)
            return 1;
        else if (sx < 0 && sy < 0)
            return 2;
        else
            return 3;
    }

    /// <summary>
    /// Set cloud parameters with game parameters object
    /// </summary>
    /// <param name="parameters">Game parameters object</param>
    public void SetParameters(int initialSpeed, int acceleration, int index, int cloudArea)
    {
        Speed = initialSpeed;
        Acceleration = acceleration;
        cloudIndex = index;

        if (_PositionFactory == null)
            _PositionFactory = new CloudsPositionFactory();
        Vector3 tTarget = _PositionFactory.RandomPosition();
        int sx = 1, sy = 1;
        switch (cloudArea)
        {
            case 1:
                sx = -1;
                break;

            case 2:
                sx = -1;
                sy = -1;
                break;

            case 3:
                sy = -1;
                break;
        }
        Target = new Vector3(Math.Abs(tTarget.x) * sx, Math.Abs(tTarget.y) * sy, 0);
    }

    /// <summary>
    /// Set type of cloud and change its sprite
    /// </summary>
    /// <param name="type">Cloud type</param>
    public void SetType(CloudType type)
    {
        Type = type;
        SetSprite();
    }

    /// <summary>
    /// Get the cloud type
    /// </summary>
    /// <returns></returns>
    public CloudType getCloudType() 
    {
        return Type;
    }

    /// <summary>
    /// Set rain to normal aniation if cloud is moving and is rainy
    /// </summary>
    public void SetRainToNormalAnimation()
    {
        Animator animator = gameObject.GetComponent<Animator>();
        animator.enabled = true;
        animator.Play("RainToNormal", 0, 0.15f);
        animator.speed = (float)(Acceleration / Speed);
    }

    /// <summary>
    /// Disable cloud animation
    /// </summary>
    internal void DisableAnimation()
    {
        gameObject.GetComponent<Animator>().enabled = false;
    }

    /// <summary>
    /// Set cloud sprite base of its type
    /// </summary>
    internal void SetSprite()
    {
        Sprite sprite;
        switch (Type)
        {
            case CloudType.Normal:
                {
                    sprite = Resources.Load("Clouds/Sprites/simple-cloud", typeof(Sprite)) as Sprite;
                    break;
                }
            case CloudType.Rainy:
                {
                    sprite = Resources.Load("Clouds/Sprites/rain-cloud", typeof(Sprite)) as Sprite;
                    break;
                }
            case CloudType.Thunder:
                {
                    sprite = Resources.Load("Clouds/Sprites/dark-cloud", typeof(Sprite)) as Sprite;
                    break;
                }
            default:
                {
                    sprite = Resources.Load("Clouds/Sprites/simple-cloud", typeof(Sprite)) as Sprite;
                    break;
                }
        }
        gameObject.GetComponent<Image>().sprite = sprite;

    }

    /// <summary>
    /// Enable moving after 1 second
    /// </summary>
    IEnumerator EnableMoving()
    {
        yield return new WaitForSeconds(1f);
        Moving = true;
        if (Type == CloudType.Rainy)
            SetRainToNormalAnimation();
    }

    /// <summary>
    /// Cloud clicked event
    /// </summary>
    public void Cloud_Clicking()
    {
        CloudsManager.Instance.hiddenDataEncoder.cloudIndex = cloudIndex;
        GameObject gameHandler = GameObject.Find("GameCanvas");
        if (Type == CloudType.Normal && Stoped)
        {
            Type = CloudType.Thunder;
            EventManager.Instance.InvokeEvent("FalseAnswerEvent");
            Clicked = true;
        }
        else if (Type == CloudType.Rainy && Stoped && !Clicked)
        {
            EventManager.Instance.InvokeEvent("TrueAnswerEvent");
            Clicked = true;
        }
        SetSprite();
    }

    /// <summary>
    /// Set cloud scale
    /// </summary>
    /// <param name="scale">Scale number</param>
    public void SetScale(float scale)
    {
        GetComponent<Transform>().localScale = new Vector3(scale, scale, 1);
    }

    #endregion

}
