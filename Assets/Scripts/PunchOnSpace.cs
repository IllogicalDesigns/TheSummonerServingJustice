using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PunchOnSpace : MonoBehaviour
{
    [SerializeField] private float punchScale = 0.9f;
    [SerializeField] private float punchDuration = 0.1f;

    [SerializeField] private float punchRotation = 0.2f;

    Tween scaleTween;
    Tween rotationTween;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            scaleTween.Kill(true);
            scaleTween = transform.DOPunchScale(Vector3.one * punchScale, punchDuration);

            rotationTween.Kill(true);
            rotationTween = transform.DOPunchRotation(Vector3.forward * Random.Range(-punchRotation, punchRotation), punchDuration);
        }   
    }
}
