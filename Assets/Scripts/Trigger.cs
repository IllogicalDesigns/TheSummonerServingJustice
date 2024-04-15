using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] string requiredTag = "";
    // Start is called before the first frame update
    void Start()
    {
        var meshRend = gameObject.GetComponent<MeshRenderer>();
        if(meshRend != null) meshRend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(requiredTag == "" || other.tag == requiredTag) {
            gameObject.SendMessage("OnRequiredTriggerEnter", other, SendMessageOptions.DontRequireReceiver);
        }   
    }

    private void OnTriggerExit(Collider other) {
        if (requiredTag == "" || other.tag == requiredTag) {
            gameObject.SendMessage("OnRequiredTriggerExit", other, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (requiredTag == "" || other.tag == requiredTag) {
            gameObject.SendMessage("OnRequiredTriggerStay", other, SendMessageOptions.DontRequireReceiver);
        }
    }
}
