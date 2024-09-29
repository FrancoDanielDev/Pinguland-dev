using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilitysUIDisplay : MonoBehaviour, IObserver
{
    [SerializeField]    private Image   _shootingCoolDown;
                        private float   _shootElapsedTime;
                        private bool    _shootBool;

    [SerializeField]    private Image   _stenchCooldown;
                        private float   _stenchElapsedTime;
                        private bool    _stentchBool;

    [SerializeField]    private Image   _mineCoolDown;
                        private float   _mineElapsedTime;
                        private bool    _mineBool;

    private void Start()
    {
        PlayerBrain.instance.Abilitys.ShootingAbility.Subscribe(this);
        _shootBool = PlayerBrain.instance.Abilitys.ShootingAbility.ReadyForShoot;
        _shootingCoolDown.fillAmount = 1;

        PlayerBrain.instance.Abilitys.MinePlacedAbility.Subscribe(this);
        _mineBool = PlayerBrain.instance.Abilitys.MinePlacedAbility.CanPlace;
        _mineCoolDown.fillAmount = 0;

        PlayerBrain.instance.Abilitys.StenchAbility.Subscribe(this);
        _stentchBool = PlayerBrain.instance.Abilitys.StenchAbility.MakeDamage;
        _stenchCooldown.fillAmount = 0;

        _stentchBool    = true;
        _mineBool       = true;
    }

    private void Update()
    {
        if (PlayerBrain.instance.Abilitys.ShootingAbility.isActiveAndEnabled)
        {
            if (!_shootBool)
            {
                _shootElapsedTime += Time.deltaTime;
                _shootingCoolDown.fillAmount = Mathf.Clamp01(_shootElapsedTime / PlayerBrain.instance.Abilitys.ShootingAbility.CoolDownTime);

                if (_shootingCoolDown.fillAmount == 1)
                    _shootBool = true;
            }
            else
                _shootingCoolDown.fillAmount = 1;
        }
        else
            _shootingCoolDown.fillAmount = 0;

        if (PlayerBrain.instance.Abilitys.StenchAbility.isActiveAndEnabled)
        {
            if (!_stentchBool)
            {
                _stenchElapsedTime += Time.deltaTime;
                _stenchCooldown.fillAmount = Mathf.Clamp01(_stenchElapsedTime / PlayerBrain.instance.Abilitys.StenchAbility.CoolDownTime);

                if (_stenchCooldown.fillAmount == 1)
                    _stentchBool = true;
            }
            else
                _stenchCooldown.fillAmount = 1;
        }
        else
            _stenchCooldown.fillAmount = 0;

        if (PlayerBrain.instance.Abilitys.MinePlacedAbility.isActiveAndEnabled)
        {
            if (!_mineBool)
            {
                _mineElapsedTime += Time.deltaTime;
                _mineCoolDown.fillAmount = Mathf.Clamp01(_mineElapsedTime / PlayerBrain.instance.Abilitys.MinePlacedAbility.CoolDownTime);

                if (_mineCoolDown.fillAmount == 1)
                    _mineBool = true;
            }
            else
                _mineCoolDown.fillAmount = 1;
        }
        else
            _mineCoolDown.fillAmount = 0;
    }

    public void CheckNotify(IObservable observable, Enum value)
    {
        if(observable is Shooting) 
        {
            _shootElapsedTime = 0;
            _shootingCoolDown.fillAmount = 0;
            _shootBool = false;
        }

        if (observable is Stench)
        {
            _stenchElapsedTime = 0;
            _mineCoolDown.fillAmount = 0;
            _stentchBool = false;
        }

        if (observable is MinePlaced)
        {
            _mineElapsedTime = 0;
            _mineCoolDown.fillAmount = 0;
            _mineBool = false;
        }
    }
}