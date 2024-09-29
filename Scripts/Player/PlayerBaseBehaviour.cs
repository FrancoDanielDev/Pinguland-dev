using UnityEngine;

public abstract class PlayerBaseBehaviour : MonoBehaviour
{
    protected PlayerBrain _brain;

    protected virtual void Start()    { _brain = GetComponent<PlayerBrain>(); }
}