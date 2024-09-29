using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class JunkRecolector : MonoBehaviour
{
    [SerializeField] private AbilitysHUB _abilitysHUB;
    [SerializeField] private int _experience;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<Junk>(out Junk j))
        {
            j.DestroyJunk();
            LevelUp(j.ExpPoints);
        }
    }

    private void LevelUp(int value)
    {
        _experience += value;

        if(_experience >= _abilitysHUB.ShootingCost)
        {
            _abilitysHUB.Unlock(Abilitys.Shooting);

            if(_experience >= _abilitysHUB.StenchCost)
            {
                _abilitysHUB.Unlock(Abilitys.Stench);

                if(_experience >= _abilitysHUB.MineCost)
                {
                    _abilitysHUB.Unlock(Abilitys.Mine);
                }
            }
        }
    }
}