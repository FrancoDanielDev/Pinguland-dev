using UnityEngine;

public class Junk : MonoBehaviour
{
    [SerializeField] private GameObject[]   _typesOfJunk;
    [SerializeField] private int            _expPoints;

    public int ExpPoints { get { return _expPoints; } private set { } }

    private void Start()
    {
        for (int i = 0; i < _typesOfJunk.Length; i++) _typesOfJunk[i].SetActive(false);
        _typesOfJunk[Random.Range(0, _typesOfJunk.Length)].SetActive(true);
    }

    public void DestroyJunk()
    {
        AudioManager.instance.Play("PickUpJunk");
        Destroy(gameObject);
    }
}
