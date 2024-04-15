using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillTargetWithEnemy : MonoBehaviour
{
    WorldSpaceMarker spaceMarker;

    void Awake() {
        spaceMarker = GetComponent<WorldSpaceMarker>();
        FillEnemy();
    }

    // Start is called before the first frame update
    void Start()
    {
        FillEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if(spaceMarker.target == null) {
            FillEnemy();
        }
    }

    private void FillEnemy() {
        var enemy = FindObjectOfType<Enemy>().transform;

        if (enemy != null) {
            spaceMarker.target = enemy;
            this.enabled = false;
        }
    }
}
