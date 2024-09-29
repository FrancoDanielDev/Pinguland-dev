using System;
using UnityEngine;

public enum Abilitys
{
    Shooting,
    Stench,
    Mine,
}

public class AbilitysHUB : PlayerBaseBehaviour, IObserver
{
    [SerializeField] private Shooting       _shootingAbility;
    [SerializeField] private Stench         _stenchAbility;
    [SerializeField] private MinePlaced     _minePlaced;

    [SerializeField] private int            _shootingCost;
    [SerializeField] private int            _stenchCost;
    [SerializeField] private int            _mineCost;


    private bool        _alreadyDead;
    public bool         alreadyDead             { get { return _alreadyDead; }      private set { } }
    public Shooting     ShootingAbility         { get { return _shootingAbility; }  private set { } }
    public MinePlaced   MinePlacedAbility       { get { return _minePlaced; }       private set { } }
    public Stench       StenchAbility           { get { return _stenchAbility; }    private set { } }

    public int          ShootingCost            { get { return _shootingCost; }     private set { } }
    public int          StenchCost              { get { return _stenchCost; }       private set { } }
    public int          MineCost                { get { return _mineCost; }         private set { } }

    protected override void Start()
    {
        base.Start();
        //_shootingAbility.enabled = false;
        _stenchAbility.enabled = false;
        _minePlaced.enabled = false;
    }

    public void Unlock(Abilitys value)
    {
        switch (value)
        {
            case Abilitys.Shooting:
                /*if (!_shootingAbility.isActiveAndEnabled)
                {
                    AudioManager.instance.Play("PLevelUp");
                    _shootingAbility.enabled = true;
                }*/
                break;
            case Abilitys.Stench:
                if (!_stenchAbility.isActiveAndEnabled)
                {
                    AudioManager.instance.Play("PLevelUp");
                    _stenchAbility.enabled = true;
                    _stenchAbility.ShowArea();
                }
                break;
            case Abilitys.Mine:
                if (!_minePlaced.isActiveAndEnabled)
                {
                    AudioManager.instance.Play("PLevelUp");
                    _minePlaced.enabled = true;
                }
                break;
        }
    }

    public void CheckNotify(IObservable observable, Enum value)
    {
        if(observable is PlayerHealth)
        {
            switch (value)
            {
                case HealthNotifies.Dead:
                    _alreadyDead = true;
                    break;
            }
        }
    }
}