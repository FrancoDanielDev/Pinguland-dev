using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnds : MonoBehaviour
{
    [SerializeField] private Island _island;
    [SerializeField] private GameObject _spawner;
    [SerializeField] private Shooting _shooting;
    [SerializeField] private AudioSource _as;

    private void OnEnable()
    {
        _island.enabled = false;
        _spawner.SetActive(false);
        _shooting.enabled = false;
        _as.volume = 0.01f;
    }
}
