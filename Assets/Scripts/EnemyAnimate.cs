using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimate : MonoBehaviour
{
    [SerializeField] Animator animator;
    CharacterController controller;
    NavMeshAgent agent;
    bool groundedLastFrame;

    const string jump = "Jump";
    const string land = "Land";
    const string speed = "Speed";
    const string isMoving = "IsMoving";
    const string pain = "Pain";

    [SerializeField] LayerMask layerMask = 1<<8;
    [SerializeField] float groundRayLength = 1f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        animator.SetFloat(speed, agent.speed);
        animator.SetBool(isMoving, agent.velocity.magnitude > 0.01f);

        bool isGrounded = IsGrounded();

        if (groundedLastFrame && !isGrounded)
            animator.SetTrigger(jump);
        else if (!groundedLastFrame && isGrounded)
            animator.SetTrigger(land);

        groundedLastFrame = IsGrounded();
    }

    private bool IsGrounded() {
        Vector3 rayStart = transform.position + transform.up;
        Debug.DrawRay(rayStart, -transform.up * groundRayLength);
        if (Physics.Raycast(rayStart, -transform.up, groundRayLength, layerMask)) {
            return true;
        }
        return false;
    }

    public void SlowImpact() {
        animator.SetTrigger(pain);
    }
}
