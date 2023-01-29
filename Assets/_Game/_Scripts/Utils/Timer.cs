using SovereignStudios.Utils;
using UnityEngine;
using static System.DateTime;
public class Timer
{
    #region Variables
    private System.DateTime startTime, endTime, pauseTime, resumeTime;
    private System.TimeSpan elapsedTime;
    private bool isTimerStopped = false;
    private bool isTimerStarted = false;
    #endregion Variables


    ~Timer()
    {
        ResetTimer();
    }

    #region Public Methods
    public void StartTimer()
    {
        if (isTimerStarted)
        {
            SovereignUtils.Log($"Timer is already Live. Restart or Reset to use.", LogType.Error);
            return;
        }
        isTimerStarted = true;
        startTime = Now;
    }
    public void StopTimer()
    {
        isTimerStopped = true;
        endTime = Now;
        elapsedTime = (endTime - startTime);
    }
    public void RestartTimer()
    {
        ResetTimer();
        StartTimer();
    }
    //public void PauseTimer()
    //{
    //    pauseTime = Now;
    //}
    //public void ResumeTimer()
    //{
    //    resumeTime = Now;
    //}
    public double GetTotalElaspedTime()
    {
        if (!isTimerStopped)
        {
            SovereignUtils.Log($"Timer is live. Can't get Total Elapsed Time. Use <color=green> GetElapsedTimeInSeconds</color> to get Live timer", LogType.Error);
            return -1;
        }
        return elapsedTime.TotalSeconds;
    }
    public double GetElapsedTimeInSeconds() => (Now - startTime).TotalSeconds;
    public double GetElapsedTimeInMilliSeconds() => (Now - startTime).TotalMilliseconds;
    #endregion Public Methods

    #region Private Methods
    private void ResetTimer()
    {
        startTime = default;
        endTime = default;
        pauseTime = default;
        resumeTime = default;
        isTimerStarted = false;
        isTimerStopped = false;
    }
    #endregion Private Methods

    #region Callbacks

    #endregion Callbacks
}
