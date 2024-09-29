using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Stench : MonoBehaviour, IObservable
{
    private List<IObserver> _observer = new List<IObserver>();
    private List<Enemy> _enemies = new List<Enemy>();
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private int _damage;
    [SerializeField] private float _damageTime;
    [SerializeField] private float _innerRadius;
    [SerializeField] private float _outerRadius;

    private float _distanceMultiplier;
    private float _distanceToEnemy;
    private bool _canMakeDamage;
    private bool _drawGizmos;
    private CircleCollider2D _circleCollider;
    private float _elapsedTime;

    public float CoolDownTime { get { return _damageTime; } private set { } }
    public bool MakeDamage { get { return _canMakeDamage; } private set { } }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.radius = _outerRadius;
        _spriteRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _damageTime)
        {
            _canMakeDamage = true;
            _elapsedTime = 0.0f;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            Notify(null);
        }
        else
        {
            _canMakeDamage = false;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        }
        transform.Rotate(0, 0, 50* Time.deltaTime);
    }

    private void Update()
    {
        if (_enemies.Count > 0 && !PlayerBrain.instance.Abilitys.alreadyDead)
        {
            if (_canMakeDamage)
            {
                for (int i = 0; i < _enemies.Count; i++)
                {
                    if (_enemies[i] != null)
                    {
                        _distanceToEnemy = Vector2.Distance(_enemies[i].transform.position, transform.position * _innerRadius);
                        _distanceMultiplier = Mathf.Clamp01(-(1 - (_distanceToEnemy / _outerRadius)));
                        _enemies[i].ReceiveDamage(_damage * _distanceMultiplier);
                    }
                }
            }
        }
    }

    public void ShowArea() { _spriteRenderer.enabled = true; }

    #region Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<Enemy>(out Enemy e))
            _enemies.Add(e);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<Enemy>(out Enemy e))
            _enemies.Remove(e);
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            //Inner Radius
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _innerRadius);

            //Outer Radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _outerRadius);
        }
    }

    [ContextMenu("Toggle Gizmos")]
    void ToggleGizmos() { _drawGizmos = !_drawGizmos; }
    #endregion

    #region Observer
    public void Subscribe(IObserver observer) { _observer.Add(observer); }

    public void UnSubscribe(IObserver observer) { _observer.Remove(observer); }

    public void Notify(Enum value)
    {
        for (int i = 0; i < _observer.Count; i++)
            _observer[i].CheckNotify(this, value);
    }
    #endregion


}