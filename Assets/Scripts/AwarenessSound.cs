using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwarenessSound : MonoBehaviour
{
    Enemy enemy;

    [SerializeField] float basePitch = 0.7f;
    [SerializeField] float baseVolume = 0.1f;
    float pitch;

    [Space]
    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy.awareness > 0) {
            if(!audioSource.isPlaying) audioSource.Play();

            if(enemy.awareness >= enemy.awarenessThreshold) {
                audioSource.Stop();
                audioSource.enabled = false;
                this.enabled = false;
            }
        } else {
            if (audioSource.isPlaying) audioSource.Stop();
        }

        if (audioSource.isPlaying) {
            audioSource.pitch = basePitch + enemy.awareness / enemy.awarenessThreshold;
            audioSource.volume = baseVolume + enemy.awareness / enemy.awarenessThreshold;
        }
    }
}
