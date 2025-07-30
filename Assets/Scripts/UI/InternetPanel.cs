using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InternetPanel : Singleton<InternetPanel>
{
    string jsonMessage = "";
    [SerializeField]
    Button ReconnectBtn;
    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        ReconnectBtn.enabled = false;
        ReconnectBtn.image.enabled = false;

    }
    public void Reconnect()
    {
        SendDataManager.Instance.SendJson(jsonMessage);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        ReconnectBtn.enabled= false;
        ReconnectBtn.image.enabled = false;
        GameManager.Instance.ResumeGame();
    }

    public void NoInternetMessage(string json)
    {
        jsonMessage=json;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        ReconnectBtn.enabled = true;
        ReconnectBtn.image.enabled = true;

        GameManager.Instance.PauseGame();
    }

}
