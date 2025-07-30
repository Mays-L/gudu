using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed;

    SpriteRenderer carColorSprite;

    GameObject highlightSpriteGameObject;

    List<Transform> endPoints;
    Transform endPoint;

    List<GameObject> entranceCollisionGameObjects;

    bool changeLine;

    public ColorSprite ColorSprite { get; private set; }

    public int Line { get; private set; }


    public bool IsSelected { get; private set; }
    public bool IsReached { get; private set; }
    public bool Go { get; set; }

    private void OnEnable()
    {
        IsReached = false;
        IsSelected = false;
        changeLine = false;

        highlightSpriteGameObject = transform.Find("HighlightSprite").gameObject;
        carColorSprite = transform.Find("Color").gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Go)
        {
            if (endPoint != null)
            {
                transform.position = GoToEndPoint();
                if ((Vector2)transform.position == (Vector2)endPoint.transform.position)
                {
                    ReachedEndPoint();
                }
            }
        }
    }

    Vector2 GoToEndPoint()
    {
        Vector2 newPosition = Vector2.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);
        
        if (changeLine)
        {
            Vector2 destinationLineTransform = new Vector2(newPosition.x, endPoints[Line].position.y);
            newPosition = Vector2.MoveTowards(newPosition, destinationLineTransform, speed * 10 * Time.deltaTime);
            if ((newPosition.y - destinationLineTransform.y) == 0)
            {
                changeLine = false;
                speed = speed * 3;
            }
        }
        return newPosition;
    }
    
    public void SetLine(int lineNumber)
    {
        Line = lineNumber;
        endPoint = endPoints[lineNumber];
    }

    public void SetSpriteColor(ColorSprite colorSprite)
    {
        ColorSprite = colorSprite;
        carColorSprite.sprite = colorSprite.sprite;
        carColorSprite.color = colorSprite.color;
    }

    public void SetEndPoints(List<Transform> endPoints, List<GameObject> entranceCollisionGameObjects)
    {
        this.endPoints = endPoints;
        this.entranceCollisionGameObjects = entranceCollisionGameObjects;
    }

    public void SelectLine(int lineNumber)
    {
        if (!IsSelected)
        {
            IsSelected = true;
            SetLine(lineNumber);
            changeLine = true;
            CarManager.Instance.NonReachedSelectedCar(this.gameObject);
        }
    }


    public void HighlightCar(bool isHighlighted)
    {
        if (isHighlighted) highlightSpriteGameObject.SetActive(true);
        else highlightSpriteGameObject.SetActive(false);
    }

    public void ReachedEndPoint()
    {
        CarManager.Instance.DestroyCar(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsReached)
        {
            if (entranceCollisionGameObjects[Line] == collision.gameObject)
            {
                CarManager.Instance.ReachedEntranceHandling(this.gameObject);
                IsReached = true;
            }
        }
    }

    public void SetSpeed(float _speed) {
        speed = _speed;
    }

    
}
