using Assets.Scripts.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Game footer behavior
/// </summary>
public class Footer : MonoBehaviour
{
    public GameObject Setting;
    public GameObject HelpPanel;
    public ScoreBoard _ScoreBoard;
    private bool enableReset = false;

    private void Start()
    {
        this.GetComponent<Image>().sprite = Resources.Load<Sprite>("General/Sprites/FooterSound");
    }

    /// <summary>
    /// Home button clicked event
    /// </summary>
    public void Home_Clicking()
    {
        GameManager.Instance.Get_Send_GameData(false);
        GameManager.Instance.ReloadGame();   
    }

    /// <summary>
    /// Repeat clicked event
    /// </summary>
    public void Repeat_Clicking()
    {
        if (enableReset)
        {
            enableReset = false;
            GameManager.Instance.Get_Send_GameData(false);
            GameManager.Instance.ResetLevel();
            _ScoreBoard.SetTime(GameManager.Instance.GameTime);
            Timers.Instance.StartTimer(2f, EnableReset);
        }
    }

    private void EnableReset()
    {
        enableReset = true;
    }

    /// <summary>
    /// Settings clicked event
    /// </summary>
    public void Settings_Clicking()
    {
        HelpPanel.SetActive(false);
        //Setting.SetActive(true);
        bool mute = AudioManager.Instance.ChangeMute();
        if(mute)
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>("General/Sprites/FooterMute" );
        else
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>("General/Sprites/FooterSound");
    }

    /// <summary>
    /// Help clicked event 
    /// </summary>
    public void Help_Clicking()
    {
        Setting.SetActive(false);
        HelpPanel.SetActive(true);
    }
}


