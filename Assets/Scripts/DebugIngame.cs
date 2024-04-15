using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugIngame : MonoBehaviour
{
    [SerializeField] NormalMove normalMove;
    [SerializeField] Image secondJumpImage;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] float debugValue = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        normalMove = FindObjectOfType<NormalMove>();
        debugValue = PlayerPrefs.GetFloat("debugValue", 1f);
        Debug.Log("debugValue in debug is " + debugValue);
    }

    private void OnDisable() {
        PlayerPrefs.SetFloat("debugValue", debugValue);
    }

    // Update is called once per frame
    void Update()
    {
        //secondJumpImage.gameObject.SetActive(normalMove.secondJump);
        textMeshProUGUI.text = normalMove.jumpCount.ToString();
    }
}
