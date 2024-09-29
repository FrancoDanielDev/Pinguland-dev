using System.Collections;
using UnityEngine;

public class Penguin : Enemy
{

    [SerializeField]    private Animator        _anim;
    [SerializeField]    private SpriteRenderer  _sr;
    [SerializeField]    private float           _speed = 2f;
    [SerializeField]    private Rigidbody2D     _rigidbody2D;
                        private Transform       _target;
                        private bool            _freeze;
                        private Vector2         _targetPos;
                        private Vector2         _direction;

    private void Start()    { _target = PlayerBrain.instance.transform; }

    private void Update()
    {
        if(!_freeze)
        {
            _anim.SetFloat("Horizontal", _direction.x);
            _anim.SetFloat("Vertical", _direction.y);
        }
    }

    private void FixedUpdate()
    {
        if (!_freeze)
        {
            _targetPos = _target.position;
            _direction = (_targetPos - _rigidbody2D.position).normalized;
            _rigidbody2D.velocity = _direction * _speed;
        }
    }

    public void ChangeTarget(Transform newTarget) { _target = newTarget; }

    public void StartFreeze(float Freezetimer) 
    { 
        StopAllCoroutines(); 
        StartCoroutine(MyFreezetimer(Freezetimer)); 
    }

    public IEnumerator MyFreezetimer(float Timeoffreeze)
    {
        var LastSpeed = _speed;
        _speed = 0;
        _freeze = true;
        yield return new WaitForSeconds(Timeoffreeze);
        _speed = LastSpeed;
        _freeze = false;
        yield return null;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<PlayerHealth>(out PlayerHealth health))
        {
            health.GetDamage();
            Die();
        }
    }

    public float GetCurrentSpeed()  { return _health; }

    public void ChangeAnimator(RuntimeAnimatorController NewAnim)
    {
        _anim.runtimeAnimatorController = NewAnim;
    }

    public void ChangeSprite(Sprite NewSprite)
    {
        _sr.sprite = NewSprite;
    }

    public void SetCurrentSpeed(float Speed)    { _speed = Speed; }

    protected override void Ouch()
    {
        base.Ouch();
        StartCoroutine(FeedBackTimer());
    }

    IEnumerator FeedBackTimer()
    {
        for (int i = 0; i < 3; i++)
        {
            _sr.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            _sr.color = Color.white;
            yield return new WaitForSeconds(0.05f);
        }

        _sr.color = Color.white;
    }

    public override void Die()
    {
        base.Die();
        _anim.SetTrigger("IsDead");
        _freeze = true;
    }
}