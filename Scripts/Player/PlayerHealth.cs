using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealthNotifies
{
    RecivedDamage,
    Dead,
}

public class PlayerHealth : PlayerBaseBehaviour, IObservable
{
                        private List<IObserver> _observers          =   new List<IObserver>();
                        private bool            _isInvulnerable     =   false;
                        private bool            _alreadyDie;
    [Range(1, 5)]
    [SerializeField]    private int             _life;
    [SerializeField]    private float           _invulnerableTime;
    [SerializeField]    private GameObject      _gameScreen;
    [SerializeField]    private GameObject      _deathScreen;

    public int TotalLife { get { return _life; } private set { } }

    public void GetDamage()
    {
        if (_isInvulnerable || _alreadyDie) return;

        _life--;
        AudioManager.instance.Play("PDamaged");
        Notify(HealthNotifies.RecivedDamage);

        if (_life <= 0)
        {
            _alreadyDie = true;
            Notify(HealthNotifies.Dead);
            _brain.Animator.SetTrigger("Dead");
            AudioManager.instance.Play("PDeath");
            _gameScreen.SetActive(false);
            _deathScreen.SetActive(true);
        }

        StartCoroutine(InvulnerableTime(_invulnerableTime));
    }

    private IEnumerator InvulnerableTime(float timer)
    {
        _isInvulnerable = true;
        _brain.SpriteRenderer.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(timer);
        _brain.SpriteRenderer.color = new Color(1, 1, 1, 1f);
        _isInvulnerable = false;
    }

    public void Notify(Enum value)
    {
        for (int i = 0; i < _observers.Count; i++)
            _observers[i].CheckNotify(this , value);
    }

    public void Subscribe(IObserver observer) { _observers.Add(observer); }

    public void UnSubscribe(IObserver observer) { _observers.Remove(observer); }
}