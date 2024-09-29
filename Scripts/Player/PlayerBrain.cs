using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Animator))]
public class PlayerBrain : MonoBehaviour
{
    public static PlayerBrain instance;

    private Rigidbody2D     _rb;
    private BoxCollider2D   _bc;
    private Animator        _anim;
    private SpriteRenderer  _sr;
    private PlayerHealth    _ph;
    private PlayerMovement  _pm;
    private AbilitysHUB     _ah;
    
    public Rigidbody2D      Rigidbody       { get { return _rb; }       private set { } }
    public BoxCollider2D    BoxCollider     { get { return _bc; }       private set { } }
    public Animator         Animator        { get { return _anim; }     private set { } }
    public SpriteRenderer   SpriteRenderer  { get { return _sr; }       private set { } }
    public PlayerHealth     Health          { get { return _ph; }       private set { } }
    public PlayerMovement   Movement        { get { return _pm; }       private set { } }
    public AbilitysHUB      Abilitys        { get { return _ah; }       private set { } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        _rb     = GetComponent<Rigidbody2D>();
        _bc     = GetComponent<BoxCollider2D>();
        _anim   = GetComponent<Animator>();
        _sr     = GetComponent<SpriteRenderer>();
        _ph     = GetComponent<PlayerHealth>();
        _pm     = GetComponent<PlayerMovement>();
        _ah     = GetComponent<AbilitysHUB>();
    }
}