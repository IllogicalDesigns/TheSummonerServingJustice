using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterBaloonLoaded : MonoBehaviour
{
    [SerializeField] Gun gun;
    [SerializeField] MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        gun = FindObjectOfType<Gun>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gun.timerCooldown <= 0)
            meshRenderer.enabled = true;
        else
            meshRenderer.enabled = false;
    }
}
