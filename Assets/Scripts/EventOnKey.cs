using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnKey : MonoBehaviour
{
    [SerializeField] KeyCode keyCode;
    [SerializeField] UnityEvent onKey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyCode)) {
            onKey.Invoke();
        }
    }
}
