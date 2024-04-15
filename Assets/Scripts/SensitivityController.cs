using UnityEngine;
using UnityEngine.UI;

public class SensitivityController : MonoBehaviour {
    [SerializeField] Slider sensSlider;     // Reference to the Slider component
    MouseLook mLook;

    private void Start() {
        LoadSensitivty();
    }

    public void ChangeSensitivty() {
        ApplySensitivity();
        SaveSensitivty();
    }

    private void ApplySensitivity() {
        if (mLook == null) mLook = FindObjectOfType<MouseLook>();
        mLook.sensitivityX = sensSlider.value;
        mLook.sensitivityY = sensSlider.value;
    }

    void LoadSensitivty() {
        sensSlider.value = PlayerPrefs.GetFloat("Sensitivity", 200f);
        ApplySensitivity();
    }

    void SaveSensitivty() {
        PlayerPrefs.SetFloat("Sensitivity", sensSlider.value);
    }
}
