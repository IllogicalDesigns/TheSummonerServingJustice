using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WinOnCollide : MonoBehaviour
{
    [SerializeField] float winOnDist = 1f;
    [SerializeField] UnityEvent win;
    [SerializeField] Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < winOnDist) {
            win.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision != null && collision.collider.CompareTag("Player")) {
            win.Invoke();
        }
    }
}
