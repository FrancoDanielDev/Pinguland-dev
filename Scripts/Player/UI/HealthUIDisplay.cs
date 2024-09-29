using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIDisplay : MonoBehaviour, IObserver
{
    [SerializeField] private GameObject _healthPrefab;
    private List<Image> _hearts = new List<Image>();
    private int _indexer;

    private void Start()
    {
        for (int i = 0; i < PlayerBrain.instance.Health.TotalLife; i++)
        {
            GameObject h = Instantiate(_healthPrefab, this.transform);
            _hearts.Add(h.GetComponent<Image>());
        }

        PlayerBrain.instance.Health.Subscribe(this);
        _indexer = _hearts.Count - 1;
    }

    public void CheckNotify(IObservable observable, Enum value)
    {
        if (observable is PlayerHealth)
        {
            switch (value)
            {
                case HealthNotifies.RecivedDamage:
                    DesactiveHearth();
                    break;
            }
        }
    }

    private void DesactiveHearth()
    {
        if (_indexer < 0) return;

        if (_hearts[_indexer].isActiveAndEnabled)
            _hearts[_indexer].enabled = false;
        else
        {
            _indexer--;
            DesactiveHearth();
        }
    }
}