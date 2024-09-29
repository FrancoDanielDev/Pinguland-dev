using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class CoconautMine : MonoBehaviour
{
    [SerializeField]    private float           _timer;
    [SerializeField]    private GameObject      _explosionPrefab;
    

    private List<Penguin>   _penguins = new List<Penguin>();

    private void Start()    
    {
        AudioManager.instance.Play("CoconautTimer");
        StartCoroutine(Effect(_timer)); 
    }

    private IEnumerator Effect(float timer)
    {
        yield return new WaitForSeconds(timer);
     
        GameObject e = Instantiate(_explosionPrefab);
        e.transform.position = transform.position;

        for (int i = 0; i < _penguins.Count; i++)
        {
            _penguins[i].ChangeTarget(PlayerBrain.instance.transform);
        }


        AudioManager.instance.Stop("CoconautTimer");
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<Penguin>(out Penguin p))
        {
            p.ChangeTarget(this.transform);
            _penguins.Add(p);
        }
    }
}