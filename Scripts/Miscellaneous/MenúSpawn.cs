using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenúSpawn : MonoBehaviour
{
    float RandomTime;
    float TimetoSpawn;
    public GameObject Pinguintospawn;
    // Start is called before the first frame update
    void Awake()
    {
        RandomTime = Random.Range(2f, 4f);
        TimetoSpawn = RandomTime;
    }

    void Update()
    {
        TimetoSpawn -= Time.deltaTime;
        if(TimetoSpawn < 0)
        {
            TimetoSpawn = RandomTime;
            Pinguintospawn.transform.position = this.transform.position;
            Instantiate(Pinguintospawn);
        }
    }
}
