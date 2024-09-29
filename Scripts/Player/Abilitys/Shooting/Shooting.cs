using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour, IObservable
{
                        private List<IObserver>     _observers          =   new List<IObserver>();
                        private Vector2             _mousePos;
                        private Vector2             _playerPos;
                        private Vector2             _direction;
                        private bool                _readyForShoot      =   true;

    [SerializeField]    private GameObject          _spitPrefab;
    [SerializeField]    private float               _coolDownTimer;

    public float    CoolDownTime    { get { return _coolDownTimer; } private set { } }
    public bool     ReadyForShoot   { get { return _readyForShoot; } private set { } }

    private void Update()
    {
        _mousePos   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _playerPos  = transform.position;
        _direction  = (_mousePos - _playerPos).normalized;

        if (Input.GetMouseButton(0) && _readyForShoot && !PlayerBrain.instance.Abilitys.alreadyDead)
        {
            GameObject Spit = Instantiate(_spitPrefab);
            Spit.transform.position = transform.position;
            Spit.GetComponent<Spit>().Factory(_direction);
            AudioManager.instance.Play("Spit");
            Notify(null);
            StartCoroutine(CoolDown(_coolDownTimer));
        }
    }

    private IEnumerator CoolDown(float timer)
    {
        _readyForShoot = false;
        yield return new WaitForSeconds(timer);
        _readyForShoot = true;
    }


    public void Notify(Enum value)
    {
        for (int i = 0; i < _observers.Count; i++)
            _observers[i].CheckNotify(this, value);
    }

    public void Subscribe(IObserver observer)   { _observers.Add(observer);    }

    public void UnSubscribe(IObserver observer) { _observers.Remove(observer); }
}