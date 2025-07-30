using Assets.Scripts.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

/// <summary>
/// End dialog behavior for represent user score
/// </summary>
public class EndDialog : MonoBehaviour
{
    #region Fields
    public Image PerformanceChart;
    public Text TextPerformance;
    public Text TextScore;
    public Text TextTruePercent;
    public Text TextProgress;

    public GameObject ResultsPanel;
    public GameObject EndTutorialPanel;
    #endregion

    #region Methods

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            ResultsPanel.SetActive(GameManager.Instance.FinishedTutorial());
            EndTutorialPanel.SetActive(!GameManager.Instance.FinishedTutorial());
        }
    }

    /// <summary>
    /// Set report parameters
    /// </summary>
    /// <param name="performance">User performance base of other users performance</param>
    /// <param name="score">User score</param>
    /// <param name="truePercent">True percent</param>
    /// <param name="progress">Progress of user base of previous games</param>
    public void SetUIValues(float performance, int score, float truePercent, float progress)
    {
        PerformanceChart.fillAmount = truePercent;
        TextPerformance.text = (int)(truePercent * 100) + "%";
        TextScore.GetComponent<Text>().text = score.ToString();
        TextTruePercent.GetComponent<Text>().text = (int)(truePercent * 100) + "%";
        TextProgress.GetComponent<Text>().text = (int)(progress * 100) + "%";
    }

    /// <summary>
    /// Restart button clicked event
    /// </summary>
    public void Restart_OnClicking()
    {
        #if UNITY_EDITOR
        GameManager.Instance.ReloadGame();
#elif UNITY_WEBGL
        //Application.OpenURL(GetDataManager.Instance.GameParameters.RedirectUrl);
        Application.ExternalEval("window.open('" + GetDataManager.Instance.GameParameters.RedirectUrl + "','_self')");
#endif

    }

    private void GotoGame()
    {
        GameManager.Instance.RestartLevel();
        gameObject.SetActive(false);
    }

    public void ReadyToGame()
    {
        GameManager.Instance.isInTutorial = false;
        GameManager.Instance.FirstEnter = false;
        GotoGame();
    }


    public void RepeatTutorial()
    {
        GameManager.Instance.isInTutorial = true;
        GameManager.Instance.FirstEnter = true;
        GotoGame();
    }

    #endregion
}
