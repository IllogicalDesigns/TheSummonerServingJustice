using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceUI : MonoBehaviour
{
    [SerializeField] Transform target;
    Transform player;
    TextMeshProUGUI textMeshProUGUI;

    // Start is called before the first frame update
    void Start()
    {
        if(target == null) target = FindAnyObjectByType<Enemy>().transform;
        player = FindAnyObjectByType<NormalMove>().transform;
        textMeshProUGUI = gameObject.GetComponent<TextMeshProUGUI>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null || player == null) {
            target = FindAnyObjectByType<Enemy>().transform;
            player = FindAnyObjectByType<NormalMove>().transform; 
            return;
        }

        textMeshProUGUI.text = Mathf.RoundToInt(Vector3.Distance(target.position, player.position)).ToString() + " M";
    }
}
