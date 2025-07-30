using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nest : MonoBehaviour
{
    [SerializeField] Sprite falseAnswerNestSprite;
    [SerializeField] Sprite trueAnswerNestSprite;
    [SerializeField] Sprite distinctNestSprite;
    [SerializeField] Sprite mouseHoverNestSprite;
    [SerializeField] Sprite defaultNestSprite;

    public bool IsDistinct { get; set; }
    public bool IsChosen { get; set; }

    public int myIndex;

    public bool _startingState;

    SpriteRenderer spriteRendererComponent;

    private void OnEnable()
    {
        IsDistinct = false;
        IsChosen = false;
        _startingState = false;
        spriteRendererComponent = GetComponent<SpriteRenderer>();
        spriteRendererComponent.sprite = defaultNestSprite;

        if (EventManager.Instance.IsInitializedEM)
        {
            EventManager.Instance.AddListeners("TouchedGameObject", TouchHandle);
            EventManager.Instance.AddListeners("StartingDistinctionEvent", ShowDistinctSprite);
            EventManager.Instance.AddListeners("EndingDistinctionEvent", ShowDefaultSprite);
        }
    }

    public void OnDisable()
    {

        if (EventManager.Instance != null && EventManager.Instance.IsInitializedEM)
        {
            EventManager.Instance.RemoveListeners("StartingDistinctionEvent", ShowDistinctSprite);
            EventManager.Instance.RemoveListeners("EndingDistinctionEvent", ShowDefaultSprite);
            EventManager.Instance.RemoveListeners("TouchedGameObject", TouchHandle);
        }
    }

    void TouchHandle(GameObject touchedNest)
    {
        NestsManager.Instance.hiddenDataEncoder.nestIndex = myIndex;
        if (this.gameObject == touchedNest && !_startingState && !IsChosen)
        {
            if (IsDistinct && !IsChosen)
            {
                EventManager.Instance.InvokeEvent("TrueAnswerEvent");
                ShowTrueAnswerSprite();
            }
            else if(!IsDistinct && !IsChosen)
            {
                EventManager.Instance.InvokeEvent("FalseAnswerEvent");
                ShowFalseAnswerSprite();
            }
            IsChosen = true;
        }
    }



    void ShowDistinctSprite()
    {
        _startingState = true;
        if(IsDistinct) spriteRendererComponent.sprite = distinctNestSprite;
    }

    void ShowDefaultSprite()
    {
        _startingState = false;
        spriteRendererComponent.sprite = defaultNestSprite;
        Timers.Instance.StartTimer("ResponseTimer", 0f);
        Timers.Instance.StartAnswerTimer(0f);
    }

    public void ShowFalseAnswerSprite()
    {
        if(!_startingState)
        spriteRendererComponent.sprite = falseAnswerNestSprite;
    }
    public void ShowTrueAnswerSprite()
    {
        if(!_startingState)
        spriteRendererComponent.sprite = trueAnswerNestSprite;
    }

    public void OnMouseEnter()
    {
        if(!IsChosen && !_startingState)
        spriteRendererComponent.sprite = mouseHoverNestSprite;
    }
    public void OnMouseExit()
    {
        if(!IsChosen  && !_startingState)
        spriteRendererComponent.sprite = defaultNestSprite;
    }
}
