using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("BASICS")]
    [SerializeField]    public  float       _health;
    [SerializeField]    protected  int         _damage;
    [SerializeField]    protected  float       _sizeofobject;
    [SerializeField]    protected  GameObject  _junkPrefab;
    [SerializeField]    protected  GameObject  _Destroyer;
    [HideInInspector]   public     bool        ItsArealEnemy;
    [HideInInspector]   public     bool        Iamnormal;
    [HideInInspector]   public     bool        Iamtheboss;

    [Header("VISUALS")]
    [SerializeField]    protected ParticleSystem _damageParticle = null;
    public Island MyInstance;

    private void Awake()
    {
        MyInstance = FindObjectOfType<Island>();

        if (Iamnormal && ItsArealEnemy)
            MyInstance.NormalEnemys.Add(this);
        else if (ItsArealEnemy)
            MyInstance.MutantEnemys.Add(this);
    }

    public void ReceiveDamage(float dmg)
    {
        _health -= dmg;

        if (_health > 0)
            Ouch();
        else
            Die();
    }

    protected virtual void Ouch()
    {
        //_damageParticle.Play();
        AudioManager.instance.Play("PenguinDamage");
    }

    public virtual void Die()
    {
        AudioManager.instance.Play("Damage");
        var ChanceOfDestroyed = Random.Range(1, 100);
        if(ChanceOfDestroyed > 1)
        {
            GameObject j = Instantiate(_junkPrefab);
            j.transform.position = this.transform.position;
        }
        else
        {
            GameObject D = Instantiate(_Destroyer);
            D.transform.position = this.transform.position;
        }
        

        if (Iamnormal && ItsArealEnemy)
            MyInstance.NormalEnemys.Remove(this);
        else if(ItsArealEnemy)
        {
            MyInstance.MutantEnemys.Remove(this);
            if(Iamtheboss)
                MyInstance.EliminateMutans();
        }
    }

    public float GetCurrentHealth() { return _health; }

    public void SetCurrentHealth(float Health) { _health = Health; }

    public float GetCurrentSize() { return _sizeofobject; }

    public void SetCurrentSize(float Size)
    {
        _sizeofobject = Size;
        this.transform.localScale = new Vector3(_sizeofobject, _sizeofobject, _sizeofobject);
    }
}