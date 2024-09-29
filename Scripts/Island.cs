using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using System;

public class Island : MonoBehaviour
{
    [Header("ENEMY BASIC")]
    [SerializeField] private float _enemySpawningDistance = 2f;
    [SerializeField] private Vector2 _spawnCD = new Vector2(2, 3);
    [SerializeField] private Vector2 _spawnQuantity = new Vector2(1, 2);
    [SerializeField] private float _buffTime = 20f;

    [Header("ENEMY LUCK")]
    [SerializeField] private float _surpriseTime = 15f;
    [SerializeField] private float _surprisePercentage = 50f;
    [SerializeField] private float _unluckyBuffPercentage = 33f;
    [SerializeField] private Vector2 _unluckyEnemies = new Vector2(1, 2);
    [Space]
    [SerializeField, ReadOnly] private float _buffTimer = 0f;
    [SerializeField, ReadOnly] private float _surpriseTimer = 0f;

    [Header("DRAGGABLES")]
    [SerializeField] private Transform _topLeftBorder;
    [SerializeField] private Transform _bottomRightBorder;
    [SerializeField] private Transform _enemySpawner;
    [SerializeField] private Enemy _enemyPenguin;

    [SerializeField] private RuntimeAnimatorController[] TypeofAnimation;
    [SerializeField] private Sprite[] Typeofsprite;
    [SerializeField] private float[] PenguinsSizes;
    [SerializeField] private float[] PenguinLifes;
    [SerializeField] private float[] PenguinSpeed;
    [SerializeField] private bool ItsTheWaveOfKing;
    [SerializeField] List<float> Characteristics;
    [SerializeField] List<Tuple<float,float,float>> Penguincombinations;
    [SerializeField] int Waves;

    public Penguin King;
    [SerializeField] private float AllWaves;
    public List<Enemy> NormalEnemys;
    public List<Enemy> MutantEnemys;

    private void Awake()
    {
        ////IA2-LINQ puntos : OrderBy y ByDescending(Ordeno de menor a mayor o mayor a menor para respetar una regla a la hora de respawnear enemigos) ToArray(cambio el resultado a un Array ya que de lo contrario da un Inumerable)
        PenguinsSizes = PenguinsSizes.OrderBy(Size => Size).ToArray();
        PenguinLifes = PenguinLifes.OrderBy(LIFE => LIFE).ToArray();
        PenguinSpeed = PenguinSpeed.OrderByDescending(SPEED => SPEED).ToArray();
        ////IA2-LINQ puntos : Zip(Combino los 3 Arrays anteriormente dados los cuales estan ordenados de tal forma que mientras más vida tenga un enemigo sera más grande y tendra menos velocidad(esta regla sera así para todos los enemigos exceptuando al boss)). ToList(cambio el resultado a un Array ya que de lo contrario da un Inumerable)
        Penguincombinations = PenguinsSizes.Zip(PenguinLifes, (Size, Life) => new { Combination = Size, EnemyLifes = Life }).Zip(PenguinSpeed,(Sizeandlife,Speed) => new Tuple<float,float,float>(Sizeandlife.Combination,Sizeandlife.EnemyLifes,Speed)).ToList();
        
    }



    private Vector2 SpawnCD
    {
        get
        {
            return new Vector2(Mathf.Max(_spawnCD.x, 0.5f), Mathf.Max(_spawnCD.y, 0.75f));
        }
        set
        {
            _spawnCD = new Vector2(Mathf.Max(value.x, 0.5f), Mathf.Max(value.y, 0.75f));
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(EnemySpawner());
    }

    private void Update()
    {
        LuckFactors();
        _buffTimer += Time.deltaTime;
        _surpriseTimer += Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Rect shore = GetIslandRect();
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(shore.center, new Vector3(shore.width, shore.height, 0f));

        Rect enemySpawning = GetIslandRect(_enemySpawningDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemySpawning.center, new Vector3(enemySpawning.width, enemySpawning.height, 0f));
    }

    #region Enemies

    private IEnumerator EnemySpawner()
    {
        if (_enemyPenguin.GetComponent<Penguin>() != null)
        {
            King = _enemyPenguin.GetComponent<Penguin>();
        }
            
        var NormalSize = King.GetCurrentSize();
        var NormalHealth = King.GetCurrentHealth();
        var NormalSpeed = King.GetCurrentSpeed();
        if (Waves > 10 && Waves < 20)
        {
            for (int i = 0; i < Random.Range(_spawnQuantity.x - 1, _spawnQuantity.y); i++)
            {
                ////IA2-LINQ puntos : Any(compruebo si efectivamente penguincombinations tiene algo en su interior(es solo una comprovacion de precaucion realmente no deberia ser necesaria))
                if (Penguincombinations.Any() != false)
                {

                    var NumbertoHave = Random.Range(0, Penguincombinations.Count - 1);
                    King.SetCurrentHealth(Penguincombinations[NumbertoHave].Item2);
                    King.SetCurrentSize(Penguincombinations[NumbertoHave].Item1);
                    King.SetCurrentSpeed(Penguincombinations[NumbertoHave].Item3);
                    _enemyPenguin.GetComponent<Penguin>().ChangeSprite(Typeofsprite[1]);
                    _enemyPenguin.GetComponent<Penguin>().ChangeAnimator(TypeofAnimation[1]);
                    _enemyPenguin.Iamnormal = false;
                    SpawnEnemy(_enemyPenguin);
                }

            }


        }
        else if (Waves >= 25)
        {
            _enemyPenguin.Iamnormal = false;
            _enemyPenguin.Iamtheboss = true;
            ////IA2-LINQ puntos Select Many : Junto la vida de todos los enemigos vivos que tengo saco un promedio y lo sumo al mayor porcentaje de vida que un mutante puede tener para tener la vida del jefe
            var MyLifes = new List<List<float>>()
            {
                NormalEnemys.Select(L => L._health).ToList(),
                MutantEnemys.Select(H => H._health).ToList()
            };
            ////IA2-LINQ puntos : Any(compruebo si efectivamente MyLifes no es nula ya que en caso de ser Nula tiraria un error y me trabaria el spawner))
            if (MyLifes.Any() != false)
            {
                var BossLife = MyLifes.SelectMany(Lifes => Lifes).ToList().Average() + Penguincombinations.Last().Item2;
                King.SetCurrentHealth(BossLife);
                Debug.Log("Vida promedio" + BossLife);
            }
            else
            {
                King.SetCurrentHealth(Penguincombinations.First().Item2);
            }
            
            
            ////IA2-LINQ puntos : Last(para agarrar la mayor vida y tamaño para el jefe(y que esta hecho de forma que solo el jefe pueda tener estos valores)) First(para agarrar la mayor velocidad para el jefe(y que esta hecho de forma que solo el jefe pueda tener estos valores))
            King.SetCurrentSize(Penguincombinations.Last().Item1);
            King.SetCurrentSpeed(Penguincombinations.First().Item3);
            King.ChangeSprite(Typeofsprite[2]);
            King.ChangeAnimator(TypeofAnimation[2]);
                
            SpawnEnemy(King);
            _enemyPenguin.Iamtheboss = false;
            Waves = -1;
        }
        else
        {
            for (int i = 0; i < Random.Range(_spawnQuantity.x - 1, _spawnQuantity.y); i++)
            {
                _enemyPenguin.GetComponent<Penguin>().ChangeSprite(Typeofsprite[0]);
                _enemyPenguin.GetComponent<Penguin>().ChangeAnimator(TypeofAnimation[0]);
                _enemyPenguin.Iamnormal = true;
                SpawnEnemy(_enemyPenguin);
            }
                
        }
        King.SetCurrentSize(NormalSize);
        King.SetCurrentHealth(NormalHealth);
        King.SetCurrentSpeed(NormalSpeed);

        yield return new WaitForSeconds(Random.Range(SpawnCD.x - 1, SpawnCD.y));
        Waves++;
        AllWaves++;
        if(AllWaves >= 60)
        {
            ////IA2-LINQ puntos : Concat(uno ambas cadenas de enemigos para desacerme de todos al mismo tiempo)
            var WypeAllEnemys = NormalEnemys.Concat(MutantEnemys).ToList();
            foreach (var Enemy in WypeAllEnemys)
            {
                if (Enemy != null)
                {
                    Enemy.Die();
                }
            }
            AllWaves = 0;
        }
        StartCoroutine(EnemySpawner());
    }

    public void EliminateMutans()
    {
        while (MutantEnemys.Count > 0)
        {
            MutantEnemys.First().GetComponent<Enemy>().Die();
        }

    }

    // ObjectControllerKiller es quien llama a esta funcion 
    public void EliminateNormalEnemys()
    {
        while (NormalEnemys.Count > 0)
        {
            NormalEnemys.First().GetComponent<Enemy>().Die();
        }
    }

    private void LuckFactors()
    {
        if (_buffTimer >= _buffTime)
        {
            _spawnQuantity.x += 1;
            _spawnQuantity.y += 2;

            SpawnCD = new Vector2(SpawnCD.x - 0.5f, Mathf.Max(SpawnCD.y - 0.25f, 0.75f));

            if (Random.Range(0, 100) < _unluckyBuffPercentage)
                _unluckyEnemies.y += 1;

            _buffTimer = 0f;
        }

        if (_surpriseTimer >= _surpriseTime)
        {
            if (Random.Range(0, 100) < _surprisePercentage)
            {
                for (int i = 0; i < Random.Range(_unluckyEnemies.x - 1, _unluckyEnemies.y); i++)
                    SpawnEnemy(_enemyPenguin);
            }

            _surpriseTimer = 0f;
        }
    }

    private void SpawnEnemy(Enemy enemy)
    {
        Rect shore = GetIslandRect(_enemySpawningDistance);
        float randomX;
        float randomY;

        if (Random.value < 0.5f)
        {
            randomX = Random.Range(shore.x, shore.x + shore.width);
            randomY = Random.value < 0.5f ? shore.y : shore.y + shore.height;
        }
        else
        {
            randomX = Random.value < 0.5f ? shore.x : shore.x + shore.width;
            randomY = Random.Range(shore.y, shore.y + shore.height);
        }

        Vector3 randomPosition = new Vector3(randomX, randomY, 0f);

        Instantiate(enemy, _enemySpawner);
        enemy.transform.position = randomPosition;
        enemy.transform.rotation = Quaternion.identity;
    }

    #endregion

    #region Island

    private Rect GetIslandRect(float offset = 1f)
    {
        Vector2 topLeft = new Vector2(_topLeftBorder.position.x, _topLeftBorder.position.y);
        Vector2 bottomRight = new Vector2(_bottomRightBorder.position.x, _bottomRightBorder.position.y);

        Vector2 newSize = (bottomRight - topLeft) * offset;
        Vector2 newCenter = (topLeft + bottomRight) / 2f;

        Rect islandRect = new Rect(newCenter - newSize / 2f, newSize);
        return islandRect;
    }

    #endregion
}
