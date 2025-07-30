using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    #region Fields and Properties
    private static T _instance;
    private bool isPersistant;
    public static T Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    #region public Methods
    public static bool IsInitialized()
    {
        return _instance != null;
    }
    #endregion

    #region protected Methods
    protected virtual void Awake()
    {
        if (isPersistant)
        {
            if (!_instance)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            _instance = this as T;
        }
        /*
        if (_instance == null)
        {
            _instance = (T)this;
        }
        else
        {
            Debug.LogError("[Singletone] You can NOT instantiate a singletone class more than once.");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        */
    }

    protected void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
    #endregion
}
