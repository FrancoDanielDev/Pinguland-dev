using UnityEngine;
using TMPro;
using System;

public class GameTime : MonoBehaviour, IObserver
{
    [SerializeField]
    private TMP_Text    _timeUI;
    private float       _totalSeconds;
    private bool        _timeOut;

    public string timeText { get { return _totalSeconds.ToTime(); } private set { } }

    private void Start()
    {
        PlayerBrain.instance.Health.Subscribe(this);
        _timeOut = false;
    }

    private void Update()
    {
        if (!_timeOut)
        {
            _totalSeconds += Time.deltaTime;
            _timeUI.text = _totalSeconds.ToTime();
        }
    }

    public void CheckNotify(IObservable observable, Enum value)
    {
        if(observable is PlayerHealth)
        {
            switch (value)
            {
                case HealthNotifies.Dead:
                    _timeOut = true;
                    break;
            }
        }
    }

    public float GetTotalSeconds()  { return _totalSeconds; }
}
