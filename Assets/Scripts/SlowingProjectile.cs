using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingProjectile : MonoBehaviour
{
    Rigidbody body;
    [SerializeField] float force;
    [SerializeField] GameObject explodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        //body.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            return;
        } else if (other.CompareTag("Enemy")) {
            other.gameObject.SendMessage("SlowImpact");

            OnHit();
        }
        else {
            OnHit();
        }
    }

    private void OnHit() {
        Instantiate(explodePrefab, transform.position, transform.rotation);

        Destroy(this.gameObject);
    }
}
