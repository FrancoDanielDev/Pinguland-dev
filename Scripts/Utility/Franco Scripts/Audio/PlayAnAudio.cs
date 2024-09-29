using UnityEngine;

public class PlayAnAudio : MonoBehaviour
{
    public void PlayAudio(string audio)
    {
        if (AudioManager.instance != null)
            AudioManager.instance.Play(audio);
        else
            Debug.Log("No Audio.");
    }
}
