using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlerKiller : MonoBehaviour
{
    public Island MyIslandManager;
    void Start()
    {
        MyIslandManager = FindObjectOfType<Island>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var Player = collision.GetComponent<PlayerBrain>();
        if (Player != null)
        {
            MyIslandManager.EliminateNormalEnemys();
            Destroy(this.gameObject);
        }
    }
}
