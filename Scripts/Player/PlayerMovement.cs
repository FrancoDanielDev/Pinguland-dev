using System;
using UnityEngine;

public class PlayerMovement : PlayerBaseBehaviour, IObserver
{
    [SerializeField]    private float           _speed;
                        private Vector2         _inputVec;
                        private bool            _canMove;

    protected override void Start()
    {
        base.Start();
        _brain.Health.Subscribe(this);
        _canMove = true;
    }

    private void Update()
    {
        _brain.Animator.SetFloat("Horizontal", _inputVec.x);
        _brain.Animator.SetFloat("Vertical", _inputVec.y);
        _brain.Animator.SetFloat("Speed", _inputVec.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (_canMove)
        {
            _inputVec.x = Input.GetAxisRaw("Horizontal");
            _inputVec.y = Input.GetAxisRaw("Vertical");
            _inputVec.Normalize();

            if (_inputVec.magnitude != 0)
                _brain.Rigidbody.MovePosition(_brain.Rigidbody.position + _inputVec * _speed * Time.fixedDeltaTime);
        }
    }

    public void CheckNotify(IObservable observable, Enum value)
    {
        if(observable is PlayerHealth)
        {
            switch (value)
            {
                case HealthNotifies.Dead:
                    _canMove = false;
                    break;
            }
        }
    }
}