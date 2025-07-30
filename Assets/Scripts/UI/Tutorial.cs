using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    GameObject StartAnimation;

    bool onProcess;

    private void OnEnable()
    {
        onProcess = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.FinishedTutorial())
        {
            GotoGame();
        }
    }

    private void GotoGame()
    {
        if (onProcess)
            return;
        else
            onProcess = true;
        GameManager.Instance.DisableUIElements();
        StartAnimation.SetActive(true);
        GameManager.Instance.isInTutorial = false;
        GameManager.Instance.FirstEnter = false;
        GameManager.Instance.HideLevel();
        GameManager.Instance.EndOfTutorial();
        Timers.Instance.StartTimer(1.5f, offProcess);

    }

    private void offProcess()
    {
        StartAnimation.SetActive(false);
        gameObject.SetActive(false);
    }

    /*private void OnDisable()
    {
        if (GameManager.Instance.IsRunningState())
        {
            GameManager.Instance.DisableUIElements();
            StartAnimation.SetActive(true);
            GameManager.Instance.EndOfTutorial();
        }
    }*/

    public void NoHelpClicked()
    {
        /*GameManager.Instance.HideLevel();
        gameObject.SetActive(false);*/
        GameManager.Instance.isInTutorial = false;
    }
}
