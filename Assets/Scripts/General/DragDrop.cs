using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Drag drop behavior
/// </summary>
public class DragDrop : MonoBehaviour, IDragHandler
{
    #region Fields

    private RectTransform _RectTransform;

    #endregion

    #region Methods

    /// <summary>
    /// Unity awake method
    /// </summary>
    public void Awake()
    {
        _RectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// On drag event
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        _RectTransform.anchoredPosition += eventData.delta;
    }
    #endregion

}
