using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private string _audioCategory;
    [SerializeField] private Slider _slider;
    [SerializeField] private AudioMixer _master;

    private string _audioKey;

    private void Start()
    {
        _audioKey = _audioCategory + "Volume";

        if (PlayerPrefs.HasKey(_audioKey))
            _slider.value = PlayerPrefs.GetFloat(_audioKey);
        else
            PlayerPrefs.SetFloat(_audioKey, _slider.value);
    }

    public void VolumeController()
    {
        float volume = 20 * Mathf.Log10(_slider.value);
        volume = _slider.value == 0 ? -80 : volume;

        _master.SetFloat(_audioCategory, volume);
        PlayerPrefs.SetFloat(_audioKey, _slider.value);
        PlayerPrefs.SetFloat(_audioCategory + "Mixer", volume);
    }
}