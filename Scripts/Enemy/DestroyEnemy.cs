using UnityEngine;

public class DestroyEnemy : MonoBehaviour
{
    public void DestroyPenguin() { Destroy(this.transform.parent.gameObject); }
}