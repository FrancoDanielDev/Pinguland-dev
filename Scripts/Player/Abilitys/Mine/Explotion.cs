using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(CircleCollider2D))]
public class Explotion : MonoBehaviour
{
    [SerializeField]    private float               _damage;
    [SerializeField]    private float               _radius;
    [SerializeField]    private float               _StunTimer;

                        private List<Enemy>         _enemies                =   new List<Enemy>();
                        private float               _distanceMultiplier;
                        private float               _distanceToEnemy;
                        private CircleCollider2D    _circleCollider;


    private void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.radius = _radius;
        Invoke("Effect", 0.25f);
    }

    private void Effect()
    {
        ////IA2-LINQ puntos : Any(comprueba si Enemies esta vacio). OrderBy(Ordena la lista de enemigos en base a su vida que luego se utilizara para separar 2 listas más adelante) 
        if (_enemies.Any())
        {
            _enemies = _enemies.OrderBy(LIFE => LIFE.GetCurrentHealth()).ToList();
            ////IA2-LINQ puntos : Where(con el where se creara una lista de todos los enemigos que independientemente en que zona de la explosion esten si tienen la mitad de vida que saca la bomba moriran al instante)
            var EnemysToDie = _enemies.Where(Life => Life.GetCurrentHealth() <= _damage/2).ToList();
            foreach (var Penguins in EnemysToDie)
            {
                if (Penguins != null)
                    Penguins.Die();
            }

            ////IA2-LINQ puntos : SkipWhile(creo una lista de enemigos skipeando a todo pinguino que haya muerto por la regla anterior)
            var RestEnemys = _enemies.SkipWhile(Life => Life.GetCurrentHealth() <= _damage / 2).ToList();
            
            foreach ( var Penguins in RestEnemys)
            {
                
                _distanceToEnemy = Vector2.Distance(_enemies[0].transform.position, transform.position);
                _distanceMultiplier = Mathf.Clamp01(-(1 - (_distanceToEnemy / _radius)));
                
                if(_distanceMultiplier <= _radius/4)
                {
                    Penguins.ReceiveDamage(_damage / 4);
                }
                else
                {
                    Penguins.ReceiveDamage(_damage * _distanceMultiplier);
                }
            }
            ////IA2-LINQ puntos : OfType( para stunear solamente quiero que se stuneen los Pinguinos no los enemigos en general ). ToList(para crear la lista de pinguinos que pueden ser stuneados)
            List<Penguin> MyPenguinsStuned = RestEnemys.OfType<Penguin>().ToList();
            foreach (var Penguin in MyPenguinsStuned)
            {
                Debug.Log(Penguin);
                Penguin.StartFreeze(_StunTimer);
            }
            
        }

        AudioManager.instance.Play("Explotion");
        Destroy(this.gameObject);
    }

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
}