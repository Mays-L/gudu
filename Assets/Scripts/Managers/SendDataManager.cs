using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Send Data to server and save data if server was unavailable
/// </summary>
public class SendDataManager : Singleton<SendDataManager>
{
    /// <summary>
    /// Server URL
    /// </summary>
    //private string Url = "127.0.0.1:3509/Getjsonresults.aspx";
    public string Url = "http://schools.maghzineh.ir/Getjsonresults.aspx";
    void Start()
    {
        #if UNITY_ANDROID
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/");

            //every 120 second send files to server
            InvokeRepeating("SendSavedFilesAndDelete", 0, 120.0f);
        #endif
    }

    /// <summary>
    /// Send Json to server
    /// </summary>
    /// <param name="json">Json format of game results object</param>
    public void SendJson(string json)
    {
        StartCoroutine(PostRequestCoroutine(json));
    }

    public void SendGameResults(GameResults gameResults)
    {
        SendJson(JsonUtility.ToJson(gameResults));
    }

    /// <summary>
    /// Post request coroutine
    /// </summary>
    /// <param name="json">Json format of game results object</param>
    private IEnumerator PostRequestCoroutine(string json)
    {
        var jsonBinary = System.Text.Encoding.UTF8.GetBytes(json);

        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();

        UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(jsonBinary);
        uploadHandlerRaw.contentType = "application/json";


        UnityWebRequest www =
            new UnityWebRequest(Url, "POST", downloadHandlerBuffer, uploadHandlerRaw);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.downloadHandler.text.IndexOf("Success") == -1)
        {
            Debug.LogError(string.Format("{0}: {1} json is: {2}", www.url, www.error, json));
#if UNITY_EDITOR
            ;
#elif UNITY_WEBGL
            SetUIMessage(json);
            #elif UNITY_ANDROID
                if (www.isNetworkError || www.downloadHandler.text.IndexOf("parsing")==-1)
                    SaveData(json);
            #endif
        }
        else
        {
            Debug.Log(string.Format("Response: {0}", www.downloadHandler.text));

        }
    }

    private void SetUIMessage(string json)
    {
        InternetPanel.Instance.NoInternetMessage(json);
    }

    /// <summary>
    /// Save data in a temp file
    /// </summary>
    private void SaveData(string json)
    {
        string folderPath = Application.persistentDataPath + "/";
        string fileName = $@"{DateTime.Now.Ticks}.txt";
        System.IO.File.WriteAllText(folderPath + fileName, json);
    }

    /// <summary>
    /// Send saved files to server
    /// </summary>
    private void SendSavedFilesAndDelete()
    {
        try
        {
            foreach (string file in Directory.GetFiles(Application.persistentDataPath + "/"))
            {
                String contents = System.IO.File.ReadAllText(file);
                SendJson(contents);
                File.Delete(file);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
}
