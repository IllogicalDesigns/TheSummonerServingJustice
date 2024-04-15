using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] float cooldown = 1f;
    [SerializeField] public float timerCooldown = 0f;
    [SerializeField] GameObject projectile;
    Camera cam;
    [SerializeField] Transform launchPoint;
    [SerializeField] float force = 10f;

    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && timerCooldown <= 0f) {
            Rigidbody clone;
            var goClone = Instantiate(projectile, cam.transform.position, cam.transform.rotation) as GameObject;
            clone = goClone.GetComponent<Rigidbody>();

            // Give the cloned object an initial velocity along the current
            // object's Z axis
            clone.velocity = cam.transform.forward * force; // transform.TransformDirection(Vector3.forward * force);

            timerCooldown = cooldown;

            audioSource.Play();
        }   

        if(timerCooldown > 0f) {
            timerCooldown -= Time.deltaTime;
        }
    }
}
