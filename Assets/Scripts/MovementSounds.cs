using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSounds : MonoBehaviour
{
    NormalMove normalMove;
    CharacterController characterController;


    [Space]
    [SerializeField] AudioSource footStepSource;
    [SerializeField] AudioClip footStepSound;
    [SerializeField] AudioClip crouchStepSound;
    Vector3 lastFootstep;
    [SerializeField] float distBetweenFootsteps = 0.5f;
    float distance;
    [SerializeField] float baseFootstepPitch = 1f;
    [SerializeField] float footstepRange = 0.2f;

    [Space]
    [SerializeField] AudioSource slidingSource;

    [Space]
    [SerializeField] bool isJumping;
    [SerializeField] AudioSource jumpSource;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip doubleJumpSound;
    [SerializeField] AudioSource landSource;

    [Space]
    [SerializeField] AudioSource breathSource;
    [SerializeField] float maxBreath = 2;
    [SerializeField] float breath;
    [SerializeField] float breathPitch;
    [SerializeField] float breathBasePitch = 1f;
    [SerializeField] float breathBaseVolume = 0.25f;
    [SerializeField] float breathBaseMaxPitch = 1.3f;

    [Space]
    [SerializeField] AudioSource rustlingSource;
    [Space]
    [SerializeField] AudioSource windSource;

    // Start is called before the first frame update
    void Start()
    {
        normalMove = GetComponent<NormalMove>();
        characterController = GetComponent<CharacterController>();

        lastFootstep = transform.position;
    }

    // Update is called once per frame
    void Update() {
        //TODO add an observer for chokeout that disables these sounds

        HandleWindAndRustling();

        ControlBreath();

        if (isJumping) {
            if (slidingSource.isPlaying) slidingSource.Stop();
            return;
        }

        if (normalMove.currentState == NormalMove.MovementState.Sliding) {
            if (!slidingSource.isPlaying) slidingSource.Play();
        }
        else {
            ChooseFootstepClip();

            PlayFootstepsBasedOnDistance();
        }
    }

    private void HandleWindAndRustling() {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (v != 0f || h != 0f) {
            if (!rustlingSource.isPlaying) rustlingSource.Play();
            if (!windSource.isPlaying) windSource.Play();
        }
        else {
            if (rustlingSource.isPlaying) rustlingSource.Stop();
            if (!windSource.isPlaying) windSource.Play();
        }
    }

    private void ControlBreath() {
        if (breath > 0) {
            breath -= Time.deltaTime;

            if (breathPitch < breathBaseMaxPitch)
                breathPitch += Time.deltaTime;

            if (!breathSource.isPlaying) breathSource.Play();
        }
        else {
            breathPitch -= Time.deltaTime;
            if (breathSource.isPlaying) breathSource.Stop();
        }

        breathSource.pitch = breathBasePitch + breathPitch;
        breathSource.volume = breathBaseVolume + breathPitch;
    }

    public void OnJump(int count) {
        isJumping = true;

        if (count == 1) {
            jumpSource.clip = doubleJumpSound;
        }
        else {
            jumpSource.clip = jumpSound;
        }

        jumpSource.Play();
    }

    public void OnLand() {
        if (isJumping) {
            isJumping = false;
            landSource.Play();
        }
    }

    private void PlayFootstepsBasedOnDistance() {
        distance = Vector3.Distance(lastFootstep, transform.position);

        if (distance > distBetweenFootsteps) {
            lastFootstep = transform.position;

            if(breath < maxBreath)
                breath += distance*0.25f;

            AugmentFootstepPitch();
            footStepSource.Play();
        }
    }

    private void AugmentFootstepPitch() {
        footStepSource.pitch = baseFootstepPitch + Random.Range(-footstepRange, footstepRange);
    }

    private void ChooseFootstepClip() {
        if (normalMove.currentState == NormalMove.MovementState.Crouching) {
            footStepSource.clip = crouchStepSound;
        }
        else {
            footStepSource.clip = footStepSound;
        }
    }
}
