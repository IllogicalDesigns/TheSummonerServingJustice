using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChokeCanvas : MonoBehaviour
{
    ChokeOut chokeOut;
    [SerializeField] GameObject chokePanel;

    [SerializeField] Slider chokeSlider;
    //[SerializeField] Slider gripSlider;

    void awake() {
        if(chokeOut == null) {
            chokeOut = FindObjectOfType<ChokeOut>();
            return;
        }

        chokeSlider.maxValue = chokeOut.maxTime;
        //gripSlider.maxValue = chokeOut.maxGrip;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (chokeOut == null) {
            chokeOut = FindObjectOfType<ChokeOut>();
            return;
        }

        chokeSlider.maxValue = chokeOut.maxTime;
        //gripSlider.maxValue = chokeOut.maxGrip;
    }

    // Update is called once per frame
    void Update()
    {
        if(chokeOut == null) {
            chokeOut = FindObjectOfType<ChokeOut>();
            return;
        }

        chokeSlider.maxValue = chokeOut.maxTime;
        //gripSlider.maxValue = chokeOut.maxGrip;

        chokePanel.SetActive(chokeOut.chokeTime > 0);

        if(chokePanel.activeInHierarchy) {
            chokeSlider.value = chokeOut.chokeTime;
            //gripSlider.value = chokeOut.grip;
        }
    }
}
