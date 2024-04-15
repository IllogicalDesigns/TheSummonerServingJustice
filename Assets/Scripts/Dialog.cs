using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] dialog;
    int currentIndex = 0;

    [SerializeField] UnityEvent onDialogRunsOut;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < dialog.Length; i++) {
            dialog[i].gameObject.SetActive(false);
        }

        dialog[0].gameObject.SetActive(true);
    }

    private void AdvanceDialog() {
        dialog[currentIndex].gameObject.SetActive(false);
        currentIndex++;

        if (currentIndex < dialog.Length) {
            dialog[currentIndex].gameObject.SetActive(true);
        } else {
            onDialogRunsOut.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown) {
            AdvanceDialog();
        }
    }
}
