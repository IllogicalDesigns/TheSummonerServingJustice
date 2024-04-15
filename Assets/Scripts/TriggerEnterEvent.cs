using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEnterEvent : MonoBehaviour
{
    [SerializeField] UnityEvent onEnter;
    [SerializeField] bool oneTime;

    public void OnRequiredTriggerEnter(Collider other) {
        onEnter.Invoke();

        if(oneTime) { this.enabled = false; }
    }
}
