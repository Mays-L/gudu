using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MessageDialog : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);
        GameManager.Instance.ResumeGame();
    }

    private void OnMouseDown()
    {
        gameObject.SetActive(false);
        GameManager.Instance.ResumeGame();
    }
}
