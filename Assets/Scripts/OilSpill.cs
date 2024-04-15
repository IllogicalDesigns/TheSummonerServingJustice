using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpill : MonoBehaviour
{
    private const string PlayerTag = "Player";
    NormalMove normalMove;

    [SerializeField] Vector3 adjustmentVector = Vector3.right;

    // Start is called before the first frame update
    void Start()
    {
        normalMove = FindAnyObjectByType<NormalMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag(PlayerTag)) {
            normalMove.SetVelocity(adjustmentVector);
        }
    }
}
