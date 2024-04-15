using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurtImpact : MonoBehaviour
{
    [SerializeField] AudioSource hurtImpactSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SlowImpact() {
        hurtImpactSource.Play();
    }
}
