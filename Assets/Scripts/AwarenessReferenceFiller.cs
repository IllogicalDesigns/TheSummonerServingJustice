using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwarenessReferenceFiller : MonoBehaviour
{
    AwarenessUI awarenessUI;
    // Start is called before the first frame update
    void Awake()
    {
        awarenessUI = gameObject.GetComponent<AwarenessUI>();
        awarenessUI.enemy = FindObjectOfType<Enemy>();
    }
}
