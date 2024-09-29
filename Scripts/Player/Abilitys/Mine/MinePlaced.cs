using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinePlaced : MonoBehaviour, IObservable
{
    [SerializeField]    private GameObject          _coconautPrefab;
    [SerializeField]    private float               _coolDownTimer;
                        private bool                _canPlace           =   true;
                        private List<IObserver>     _observers          =   new List<IObserver>();

    public float CoolDownTime { get { return _coolDownTimer; } private set { } }
    public bool CanPlace { get { return _canPlace; } private set { } }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1) && _canPlace && !PlayerBrain.instance.Abilitys.alreadyDead)
        {
            GameObject coconaut = Instantiate(_coconautPrefab);
            coconaut.transform.position = transform.position;
            Notify(null);
            StartCoroutine(CoolDown(_coolDownTimer));
        }
    }

    private IEnumerator CoolDown(float timer)
    {
        _canPlace = false;
        yield return new WaitForSeconds(timer);
        _canPlace = true;
    }

    public void Subscribe(IObserver observer) { _observers.Add(observer); }

    public void UnSubscribe(IObserver observer) { _observers.Remove(observer); }

    public void Notify(Enum value)
    {
        for (int i = 0; i < _observers.Count; i++)
            _observers[i].CheckNotify(this, value);
    }
}