using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour {
    [SerializeField] Slider volumeSlider;     // Reference to the Slider component
    float multiplier = 10.0f;

    private void Start() {
        LoadVolume();
    }

    public void ChangeVolume() {
        float roundedNumber = Mathf.Round(volumeSlider.value * multiplier) / multiplier;
        volumeSlider.value = roundedNumber;

        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }

    void LoadVolume() {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = volumeSlider.value;
    }

    void SaveVolume() {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
}