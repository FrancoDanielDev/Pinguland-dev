using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCut : MonoBehaviour
{
    public GameObject Objecttodestroy;

    public void Death()
    {
        Destroy(Objecttodestroy.gameObject);
    }
}
