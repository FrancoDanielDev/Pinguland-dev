using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Spit : MonoBehaviour
{
                        private Rigidbody2D     _rigidbody;
    [SerializeField]    private float           _speed;
    [SerializeField]    private int             _damage;

    public void Factory(Vector2 direction)
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(direction);
        _rigidbody.AddForce(direction * _speed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<Enemy>(out Enemy e)) 
        {
            AudioManager.instance.Play("SpitDamage");
            e.ReceiveDamage(_damage);
            Destroy(gameObject);
        }
        else if(collision.gameObject.layer == 6)
        {
            AudioManager.instance.Play("SpitDamage");
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()    { Destroy(this.gameObject); }
}