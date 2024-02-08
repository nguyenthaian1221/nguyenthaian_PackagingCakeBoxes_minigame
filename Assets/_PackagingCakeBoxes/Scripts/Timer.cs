using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{

    public static Timer Instance;

    private TMP_Text _timerText;

    enum TimerType { Countdown, Stopwatch }

    [SerializeField] private TimerType timerType;

    [SerializeField] private float timeToDisplay = 45.0f;

    public float TimeToDisplay { get { return timeToDisplay; } }

    private bool _isRunning;

    private void Awake()
    {
        Instance ??= this;
        _timerText = GetComponent<TMP_Text>();


    }


    private void OnEnable()
    {
        EventManager.TimerStart += EventManagerOnTimerStart;
        EventManager.TimerStop += EventManagerOnTimerStop;
        EventManager.TimerUpdate += EventManagerOnTimerUpdate;
        EventManager.TimerPausing += EventManagerOnTimerPasuing;
    }

    private void OnDisable()
    {
        EventManager.TimerStart -= EventManagerOnTimerStart;
        EventManager.TimerStop -= EventManagerOnTimerStop;
        EventManager.TimerUpdate -= EventManagerOnTimerUpdate;
        EventManager.TimerPausing -= EventManagerOnTimerPasuing;
    }


    private void EventManagerOnTimerUpdate(float value) => timeToDisplay += value;


    private void EventManagerOnTimerStop() => _isRunning = false;
    private void EventManagerOnTimerPasuing() => _isRunning = false;



    private void EventManagerOnTimerStart() => _isRunning = true;


    private void Update()
    {
        if (!_isRunning) { return; }
        if (timerType == TimerType.Countdown && timeToDisplay < 0.0f)
        {

            EventManager.OnTimerStop();
            return;

        }
        timeToDisplay += timerType == TimerType.Countdown ? -Time.deltaTime : Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds(timeToDisplay);
        _timerText.text = timeSpan.ToString(@"ss\:ff");
    }

    public void ResetTimer()
    {
        _isRunning = true;
        timeToDisplay = 45f;
    }

}
