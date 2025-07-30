using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Diagnostics;

public class Timers : Singleton<Timers>
{
    private Dictionary<string, float> _timersDict;
    private List<string> _timersNameList;

    private Stopwatch myWatch;
    private long SWwaitTime = 0;

    private int maximumTimerCounterNumber = 200;
    public int TimerCounterStartNumber { get; private set; }


    private void OnEnable()
    {
        _timersDict = new Dictionary<string, float>();
        _timersNameList = new List<string>();
        myWatch = new Stopwatch();
    }


    /// <summary>
    /// Starting the timer counter for the game timer. 
    /// It can Invoke everySecondsEvent every second.
    /// When it finished, the finishedTimerCounterEvent will Invoke.
    /// </summary>
    public void StartTimeCounter(int amountOfTimeInSeconds, string finishedTimerCounterEvent, string everySecondsEvent = null)
    {
        TimerCounterStartNumber = amountOfTimeInSeconds > 0 ? amountOfTimeInSeconds : 0;
        TimerCounterStartNumber = TimerCounterStartNumber < maximumTimerCounterNumber ? TimerCounterStartNumber : maximumTimerCounterNumber;
        StartCoroutine(StartGameTimer(TimerCounterStartNumber, finishedTimerCounterEvent, everySecondsEvent));
    }
    IEnumerator StartGameTimer(int timerCounterStartNumber, string finishedEvent, string eachSecondEvent = null)
    {
        int counter = timerCounterStartNumber;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            if (eachSecondEvent != null) EventManager.Instance.InvokeEvent(eachSecondEvent);
            counter--;
        }
        EventManager.Instance.InvokeEvent(finishedEvent);
    }


    /// <summary>
    /// Start a timer then invoke an event
    /// </summary>
    public void StartTimer(float amountOfTimeInSecondsToWait, string nameOfEventToInvoke)
    {
        /// Problem: If the Coroutine was started lately (e.g., in Tutorial mode), StartCorouting follows the previous action => It might need a StopCoroutine to ready for the new action
        if (amountOfTimeInSecondsToWait > 0) StartCoroutine(Counting(nameOfEventToInvoke, amountOfTimeInSecondsToWait));
    }
    IEnumerator Counting(string nameOfEvent, float amountOfTimeInSecondsToWait)
    {
        yield return new WaitForSeconds(amountOfTimeInSecondsToWait);
        EventManager.Instance.InvokeEvent(nameOfEvent);
    }

    /// <summary>
    /// Start a timer then invoke an event
    /// </summary>
    public void StartTimer(float amountOfTimeInSecondsToWait, UnityAction actionToInvoke)
    {
        if (amountOfTimeInSecondsToWait > 0) StartCoroutine(Count(actionToInvoke, amountOfTimeInSecondsToWait));
    }
    IEnumerator Count(UnityAction actionToInvoke, float amountOfTimeInSecondsToWait)
    {
        yield return new WaitForSeconds(amountOfTimeInSecondsToWait);
        actionToInvoke.Invoke();
    }


    /// <summary>
    /// These Timer will start by calling StartTimer and will stop and return the amount of the time that is passed
    /// (in miliSeconds) by calling StopTimer
    /// </summary>
    /// <param name="nameOfTimer"></param>
    public void StartTimer(string nameOfTimer, float timeToWait = 0f)
    {
        if (!_timersDict.ContainsKey(nameOfTimer))
        {
            _timersDict.Add(nameOfTimer, Time.time + timeToWait);
            _timersNameList.Add(nameOfTimer);
        }
        else
        {
            _timersDict[nameOfTimer] = Time.time + timeToWait;
        }
    }
    public float StopTimer(string nameOfTimer)
    {
        float timePassed;
        myWatch.Stop();
        if (_timersDict.ContainsKey(nameOfTimer))
        {
            timePassed = Time.time - _timersDict[nameOfTimer];
            _timersDict.Remove(nameOfTimer);
            _timersNameList.Remove(nameOfTimer);
            return timePassed;
        }
        else
        {
            //Debug.LogError("There is no timer with the name " + nameOfTimer);
            return -1f;
        }
        
    }
    public float GetTimer(string nameOfTimer)
    {
        float timePassed;
        if (_timersDict.ContainsKey(nameOfTimer))
        {
            timePassed = Time.time - _timersDict[nameOfTimer];
            return timePassed;
        }
        else
        {
            //Debug.LogError("There is no timer with the name " + nameOfTimer);
            return -1f;
        }

    }

    public void StartAnswerTimer(float timeToWait = 0f)
    {
        myWatch.Reset();
        myWatch.Restart();
        SWwaitTime = (long)(timeToWait*1000f);
    }
    public long GetAnswerTimer()
    {
        return myWatch.ElapsedMilliseconds - SWwaitTime;
    }

    private void OnDisable()
    {
        while(_timersDict.Count > 0)
        {
            string nameOfTimer = _timersNameList[_timersNameList.Count - 1];
            _timersDict.Remove(nameOfTimer);
            _timersNameList.Remove(nameOfTimer);
        }
    }
}
