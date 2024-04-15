using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSounds : MonoBehaviour
{
    [SerializeField] AudioSource footStepSource;
    [SerializeField] float baseFootstepPitch = 1f;
    [SerializeField] float footstepRange = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AugmentFootstepPitch() {
        footStepSource.pitch = baseFootstepPitch + Random.Range(-footstepRange, footstepRange);
    }

    public void PlayFootstep() {
        AugmentFootstepPitch();
        footStepSource.Play();
    }
}
