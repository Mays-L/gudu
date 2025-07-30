using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wagon : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer trainImage;

    [SerializeField]
    SpriteRenderer colorImage;

    bool EnterCollider = false;

    public int myIndex = 0;

    private bool IsStatic = false;

    public bool IsAnswer { get; internal set; }

    internal void SetColor(Sprite color)
    {
        colorImage.sprite = color;
    }

    internal void SetSprite(Sprite sprite)
    {
        trainImage.sprite = sprite;
    }


    public void OffSprite()
    {
        trainImage.enabled = false;
    }

    public void OnSprite()
    {
        trainImage.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Light" && !EnterCollider && !IsStatic)
        {
            EnterCollider = true;
            WagonsManager.Instance.WagonGoIn(gameObject);
        }
        else if(collision.gameObject.name == "TrainNew"){
            EventManager.Instance.InvokeEvent("NewWagonEnter");
        }
    }
    internal void SetOffWagon()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    internal void SetOnWagon()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color32(231, 209, 0, 255);
    }
    public void SetStatic()
    {
        IsStatic = true;
        gameObject.GetComponent<Animator>().enabled = false;
    }

}
