using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpHand : MonoBehaviour
{
    public Vector3 AnswerPosition;

    [SerializeField]
    int MoveStep = 10;

    [SerializeField]
    Animator animation;

    [SerializeField]
    GameObject Dialog;
    [SerializeField]
    GameObject Hand;

    private void Start()
    {
        gameObject.transform.localPosition = new Vector3(0 , 0, 0);
    }
    private void Update()
    {
        AnswerPosition = GameManager.Instance.GetAnswerPosition();
        Vector3 position = gameObject.transform.localPosition;
        if (GameManager.Instance.IsInHideLevel() || AnswerPosition.z==10)
        {
            animation.SetBool("click", false);
            Hand.GetComponent<SpriteRenderer>().enabled = false;
            Dialog.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            Hand.GetComponent<SpriteRenderer>().enabled = true;
            Dialog.GetComponent<SpriteRenderer>().enabled = true;

            if (position.y != AnswerPosition.y)
            {
                //Move toward verticaly
                gameObject.transform.localPosition =
                    Vector3.MoveTowards(position, new Vector3(position.x, AnswerPosition.y, position.z), MoveStep);
                animation.SetBool("click", false);
                Dialog.GetComponent<SpriteRenderer>().enabled = false;

            }
            else if (position.x != AnswerPosition.x)
            {
                //Move toward horizontaly
                gameObject.transform.localPosition =
                    Vector3.MoveTowards(position, new Vector3(AnswerPosition.x, position.y, position.z), MoveStep);
                animation.SetBool("click", false);
                Dialog.GetComponent<SpriteRenderer>().enabled = false;

            }
            else
            {
                //Active click
                animation.SetBool("click", true);
            }
        }
    }


}
