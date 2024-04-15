using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwarenessUI : MonoBehaviour
{
    [SerializeField] Slider slider;
    public Enemy enemy;

    bool thresholdReached;

    private void Awake () {
        enemy = FindAnyObjectByType<Enemy>();
    }

    private void Update() {
        if(enemy == null) {
            enemy = FindAnyObjectByType<Enemy>(); 
            return;
        }

        if(enemy.isAware) transform.parent.gameObject.SetActive(false);

        slider.maxValue = enemy.awarenessThreshold;
        slider.value = enemy.awareness;

        ShowHideSliderBasedOnAwareness();
    }

    private void ShowHideSliderBasedOnAwareness() {
        if (enemy.awareness >= enemy.awarenessThreshold) thresholdReached = true;

        if (enemy.awareness <= 0)
            slider.gameObject.SetActive(false);
        else if (thresholdReached)
            slider.gameObject.SetActive(false);
        else
            slider.gameObject.SetActive(true);
    }
}
