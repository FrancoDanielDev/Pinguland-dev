using UnityEngine;

public enum FramesRate
{
    NoLimit = 0,
    Limit24 = 24,
    Limit30 = 30,
    Limit60 = 60,
    Limit120 = 120,
}

public class FrameRateLimit : MonoBehaviour
{
    public  FramesRate  limit;

    private void Awake()    { Application.targetFrameRate = (int)limit; }
}