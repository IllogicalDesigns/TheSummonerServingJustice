using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NormalMove : MonoBehaviour {
    private const KeyCode jumpKey = KeyCode.Space;
    private const KeyCode crouchSlideKey = KeyCode.LeftControl;
    private const KeyCode altCrouchSlideKey = KeyCode.C;
    private const string verticalAxis = "Vertical";
    private const string horizontalAxis = "Horizontal";
    CharacterController characterController;

    [SerializeField] float speed = 5;

    private Vector3 velocity;
    public float gravity = -9.8f; // Gravity Force

    [Space]
    [SerializeField] float jumpForce = 100;
    float jumpTimer = 0f;
    [SerializeField] private AnimationCurve jumpForceOverTimecurve;
    [SerializeField] bool isJumping;
    public int jumpCount = 0;
    public bool secondJump;

    [Space]
    [SerializeField] float slideSpeed = 5f;
    float slideTimer = 0f;
    [SerializeField] private AnimationCurve slideOverTimecurve;

    [Space]
    float wallRunTimer = 0f;
    [SerializeField] float wallRunSpeed = 5f;
    [SerializeField] private AnimationCurve WallSpeedOverTimecurve;
    [SerializeField] private AnimationCurve WallGravityOverTimecurve;
    [SerializeField] float wallJumpForce = 5f;
    [SerializeField] float lockOutWallRunAfterJump = 0.5f;
    float wallJumpLock = 0f;

    float v, h;

    bool m_Started;
    [Space]
    [SerializeField] LayerMask m_LayerMask = 1 << 0;
    [SerializeField] Vector3 boxSize = Vector3.one;
    [SerializeField] float boxRightDist = 1f;

    float originalSpeed;
    float originalCrouchSpeed;
    float originalHeight;
    [SerializeField] float crouchHight = 0.5f;

    [Space]
    [SerializeField] float dodgeTimer, dodgeCooldown = 2f;
    [SerializeField] float dodgeSpeed = 20f;
    [SerializeField] private AnimationCurve dodgeForceOverTimecurve;
    Vector3 dodgeVector;

    [SerializeField] bool isAllowedToDoubleJump = true;

    bool isPaused;

    Camera cam;

    public enum MovementState {
        Idle,
        Running,
        WallRunning,
        Crouching,
        Sliding
    }

    [Space]
    [SerializeField] public bool wallRunLeft, wallRunRight;
    [SerializeField] public MovementState currentState;

    public void SetVelocity(Vector3 adjustment) {
        characterController.Move(adjustment * Time.deltaTime);
    }

    // called in PlayerControllers' FixedUpdate which is called 60x per second
    public void Move(float v, float h, float _speed) {
        Vector3 forwardVector = transform.forward * v;
        Vector3 rightVector = transform.right * h;

        // Combine forward and right vectors, capping the magnitude at 1
        Vector3 direction = Vector3.ClampMagnitude(forwardVector + rightVector, 1);

        characterController.Move((direction * _speed) * Time.deltaTime);
    }

    void ApplyGravity(float _gravity) {
        velocity.y += _gravity * Physics.gravity.y * Time.deltaTime;

        if (!characterController.isGrounded)
            velocity.y -= _gravity * Time.deltaTime;
        else velocity.y = 0;

        characterController.Move((velocity) * Time.deltaTime);
    }

    // Start is called before the first frame update
    void Start() {
        characterController = gameObject.GetComponent<CharacterController>();
        m_Started = true;

        originalSpeed = speed;
        originalCrouchSpeed = speed / 2;
        originalHeight = characterController.height;

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        if (isPaused) return;

        v = Input.GetAxis(verticalAxis);
        h = Input.GetAxis(horizontalAxis);

        //Detect if a wall running wall is to the left or right of camera
        wallRunRight = DetectWallRunning(cam.transform.position + transform.right * boxRightDist);
        wallRunLeft = DetectWallRunning(cam.transform.position + transform.right * -boxRightDist);

        if (wallJumpLock > 0)
            wallJumpLock -= Time.deltaTime;

        //if (dodgeTimer > 0)
        //    dodgeTimer -= Time.deltaTime;

        if (!wallRunLeft && !wallRunRight) wallRunTimer = 0;

        if (characterController.isGrounded) velocity.y = 0;

        if (GetCrouchKey() && v > 0.5f && currentState != MovementState.Crouching) {
            // Handle sliding behavior when player presses crouch and is moving forward, well not already crouching
            HandleSlide();
        } else if (GetCrouchKey()) {
            // Handle Crouching behavior when player presses crouch and is NOT moving forward
            HandleCrouch();
        } else if (/*!characterController.isGrounded && v > 0.5f &&*/ (wallRunLeft || wallRunRight) /*&& wallJumpLock <= 0*/) {
            // Handle wall running behavior when player is in air, moving forward and wall running
            HandleWallRunning();
        } else {
            HandleRunning();
        }

        if (isPaused) return;

        if (currentState == MovementState.Running || currentState == MovementState.Crouching) {
            Move(v, h, speed);
            ApplyGravity(gravity);
            Jumping();

            if (currentState == MovementState.Running)
                Dodging();

        }
        else if (currentState == MovementState.Sliding) {
            Move(1, h, slideSpeed * slideOverTimecurve.Evaluate(slideTimer));
            ApplyGravity(gravity);
            Jumping();
        }
        else if (currentState == MovementState.WallRunning) {
            Move(1, h, wallRunSpeed * WallSpeedOverTimecurve.Evaluate(slideTimer));
            Jumping();
            if (isJumping) ApplyGravity(WallGravityOverTimecurve.Evaluate(wallRunTimer) * gravity);
        }
    }

    private void HandleWallRunning() {
        if (currentState != MovementState.WallRunning) {
            wallRunTimer = 0f;
        }

        wallRunTimer += Time.deltaTime;
        currentState = MovementState.WallRunning;
        velocity.y = 0;
    }

    private void HandleRunning() {
        currentState = MovementState.Running;
        characterController.height = originalHeight;
        speed = originalSpeed;
    }

    private void HandleCrouch() {
        currentState = MovementState.Crouching;
        characterController.height = crouchHight;
        speed = originalCrouchSpeed;
    }

    private void HandleSlide() {
        if (currentState != MovementState.Sliding) slideTimer = 0f;
        slideTimer += Time.deltaTime;

        currentState = MovementState.Sliding;
        characterController.height = crouchHight;
    }

    private static bool GetCrouchKey() {
        return (Input.GetKey(crouchSlideKey) || Input.GetKey(altCrouchSlideKey));
    }

    private bool DetectWallRunning(Vector3 boxPosition) {
        bool detectedWall = false;
        Collider[] hitColliders = Physics.OverlapBox(boxPosition, boxSize / 2, Quaternion.identity, m_LayerMask);
        foreach (Collider col in hitColliders) {
            if (col.CompareTag("WallRun")) { detectedWall = true; break; }
        }

        if (detectedWall) 
            return true;

        return false;
    }

    void Dodging() {
        //if(Input.GetKeyDown(KeyCode.LeftShift) && dodgeTimer <= 0) {
        //    Vector3 forwardVector = transform.forward * v;
        //    Vector3 rightVector = transform.right * h;

        //    // Combine forward and right vectors, capping the magnitude at 1
        //    //Vector3 direction = Vector3.ClampMagnitude(forwardVector + rightVector, 1);
        //    dodgeVector = Vector3.zero;
        //    dodgeTimer = dodgeCooldown;
        //} else if(dodgeTimer > 0) {
        //    characterController.Move(dodgeVector * dodgeSpeed * dodgeForceOverTimecurve.Evaluate(dodgeTimer) * Time.deltaTime);
        //}
    }

    void Jumping() {
        //TODO Fix Double jumping, its not consistent.  The second jump is consumed 
        //if (!isAllowedToDoubleJump) secondJump = false;  //Disable double jump as it is unreliable

        if (IsGrounded()) {
            ResetJumpVariables();
        }

        // If we're currently jumping and our jump timer has exceeded the maximum duration allowed by the curve, stop jumping
        float maxJumpTime = jumpForceOverTimecurve.keys[jumpForceOverTimecurve.keys.Length - 1].time;
        if (isJumping && jumpTimer > maxJumpTime) {
            StopJumping();
            return;
        }

        // When the jump key is pressed down and either the character is on the ground or it's their second jump attempt, start jumping
        if (Input.GetKeyDown(jumpKey) && CanJump()) {
            //if (!IsGrounded() && secondJump && jumpCount == 1) secondJump = false;
            StartJump();
        }

        // Only continue executing this method if we're currently jumping
        if (!isJumping) return;
        else {
            // When the jump key is released, stop jumping
            if (Input.GetKeyUp(jumpKey)) {
                StopJumping();
                //jumpTimer = 0f; //Why are we resetting the timer here
            }

            // While holding down the jump key, apply upward force to the character controller using an animation curve over time
            if (!Input.GetKey(jumpKey)) {
                //jumpTimer = 0f;
                StopJumping();
                return;
            }

            velocity.y = 0f;

            jumpTimer += Time.deltaTime;
            characterController.Move(new Vector3(0, (jumpForceOverTimecurve.Evaluate(jumpTimer) * jumpForce) * Time.deltaTime, 0f));
        }

        bool IsGrounded() {
            return characterController.isGrounded || currentState == MovementState.WallRunning;
        }

        void StopJumping() {
            isJumping = false;
            jumpTimer = 0f;
        }

        void ResetJumpVariables() {
            gameObject.SendMessage("OnLand", jumpCount);
            isJumping = false;
            jumpTimer = 0f;
            //secondJump = true;
            jumpCount = 0;
        }

        bool CanJump() {
            return (jumpCount <= 1 || characterController.isGrounded);
        }

        void StartJump() {
            gameObject.SendMessage("OnJump", jumpCount);
            isJumping = true;
            jumpTimer = 0f;
            jumpCount++;
        }
    }

    // FixedUpdate is called 60x per second
    //void Update() {

    //}

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos() {

        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started) {
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.color = wallRunRight ?  Color.green : Color.red;
            Gizmos.DrawWireCube(cam.transform.position + transform.right * boxRightDist, boxSize);

            Gizmos.color = wallRunLeft ? Color.yellow : Color.magenta;
            Gizmos.DrawWireCube(cam.transform.position + transform.right * -boxRightDist, boxSize);
        }
    }
}

