using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ChokeOut : MonoBehaviour
{
    [SerializeField] UnityEvent onChokeHoldStart;
    [SerializeField] UnityEvent onChokeHoldEnd;
    [SerializeField] UnityEvent onWin;

    [SerializeField] CinemachineVirtualCamera virtualChokeCamera;

    CharacterController controller;
    [SerializeField] float speed = 5f;
    [SerializeField] float sideSpeed = 1f;
    [SerializeField] float sideWalkSpeed = 1f;
    [SerializeField] float sensitivityModifier = 0.1f;
    [SerializeField] AnimationCurve energyCurve;
    [SerializeField] AnimationCurve energySideCurve;

    bool inChokehold;

    MouseLook mouseLook;

    [Space]
    [SerializeField] float maxWallSearchDist = 5f;
    [SerializeField] LayerMask layerMask;

    private const string horizontalAxis = "Horizontal";
    [SerializeField]  private float TimeLostOnSlowingImpact = 0.5f;
    public float RotationSmoothTime = 0.25f;
    private float targetRotationAngle;
    private Quaternion currentRotation;

    int randIndex = 0;
    int randVar = 0;
    int numOfHits = 4;
    [SerializeField] float repeatTime = 0.5f;

    [SerializeField] public float maxTime = 10f;
    [SerializeField] public float minTime = 4f;
    [SerializeField] public float startTime = 2f;
    [SerializeField] public float timeLostOnEachAttack = 2f;
    public float chokeTime;

    [SerializeField] public float maxGrip = 50f;
    [SerializeField] public float grip = 100f;
    [SerializeField] float gripLoss = 5f;
    NavMeshAgent agent;

    [SerializeField] Transform destination;
    [SerializeField] float increaseSpeed = 2f;
    [SerializeField] float decreaseSpeed = 1f;

    [SerializeField] Animator animator;

    [SerializeField] AudioSource bassSource;
    [SerializeField] AudioSource chockingSource;

    [SerializeField] float timeInChokeHold;
    [SerializeField] AnimationCurve decreaseTimeOverTimeCurve;

    // Start is called before the first frame update
    void Start()
    {
        grip = maxGrip;
        controller = GetComponent<CharacterController>();
        mouseLook = FindObjectOfType<MouseLook>();

        //InvokeRepeating(nameof(RandomVar), repeatTime, repeatTime);
    }

    //public void RandomIndex() {
    //    randIndex = Random.Range(0, numOfHits-1);
    //}

    public void RandomVar() {
        randVar = Random.Range(0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        if(!inChokehold) { return; }

        timeInChokeHold += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space)) {
            bassSource.Play();
            chokeTime += increaseSpeed * Time.deltaTime;
        }

        chokeTime -= (decreaseTimeOverTimeCurve.Evaluate(timeInChokeHold) * decreaseSpeed) * Time.deltaTime;

        if (chokeTime >= maxTime) {
            onWin.Invoke();
            chockingSource.Stop();
            inChokehold = false;
        }

        if (chokeTime <= 0) {
            ExitChokehold();
        }

        //float h = Input.GetAxis(horizontalAxis);

        //controller.Move(transform.right * ((randVar > 50 ? 1 : -1) * speed) * Time.deltaTime);

        agent.speed = 1f;
        agent.SetDestination(destination.position);
        {
            //chokeTime += Time.deltaTime;

            //controller.Move(transform.forward * energyCurve.Evaluate(chokeTime) * speed *  Time.deltaTime);

            //float h = Input.GetAxis(horizontalAxis);
            //Vector3 rightVector = transform.right * h;

            //// Combine forward and right vectors, capping the magnitude at 1
            //Vector3 direction = Vector3.ClampMagnitude(rightVector, 1);

            //controller.Move((direction * energySideCurve.Evaluate(chokeTime) * sideWalkSpeed) * Time.deltaTime);

            //bool hasHit = false;
            //var hitsColliders = Physics.OverlapSphere(transform.position, 1f);
            //for (int i = 0; i < hitsColliders.Length; i++) {
            //    if (hitsColliders[i].gameObject == this.gameObject) continue;
            //    hasHit = true;
            //    break;
            //}

            //if (hasHit)
            //{
            //    grip -= gripLoss * Time.deltaTime;
            //}

            //if(chokeTime >= 10) {
            //    onWin.Invoke();
            //    inChokehold = false;
            //}

            //if(grip < 0) {
            //    ExitChokehold();
            //}

            //// Rotate around the X axis based on Mouse Y input
            ////float yRotation = (isInverted ? -1 : 1) * Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
            ////currentXRotation -= yRotation; // Subtract because we are rotating around the negative Z axis
            ////currentXRotation = Mathf.Clamp(currentXRotation, minClampAngle, maxClampAngle);

            ////// Apply the X axis rotation to the camera transform
            ////camTransform.localRotation = Quaternion.Euler(-currentXRotation, 0f, 0f);

            //// Rotate around the Y axis based on Mouse X input
            //float xRotation = Input.GetAxis("Mouse X") * (mouseLook.sensitivityX * sensitivityModifier) * Time.deltaTime;

            //// Apply the Y axis rotation to the player transform
            //transform.localRotation *= Quaternion.Euler(0f, xRotation, 0f);


            //Vector3[] hits = new Vector3[numOfHits];

            ////hits[0] = CastRayForward();
            //hits[0] = CastRayAtAngle(45);
            //hits[0] = CastRayAtAngle(90);
            //hits[1] = CastRayAtAngle(-90);
            //hits[3] = CastRayAtAngle(-45);

            ////float bestDist = 9999;
            ////int bestIndex = 0;
            ////for (int i = 0; i < numOfHits; i++) {
            ////    float dist = hits[i] == Vector3.zero ? 9999 : Vector3.Distance(transform.position, hits[i]);

            ////    if (dist < bestDist) {
            ////        bestDist = dist;
            ////        bestIndex = i;
            ////    }
            ////}

            //Vector3 directionToWorldPoint = (hits[randIndex] - transform.position).normalized;
            //controller.Move(directionToWorldPoint * sideSpeed * Time.deltaTime);
        }
    }

    public void SlowImpact() {
        maxTime = maxTime - TimeLostOnSlowingImpact;
        if (maxTime < minTime) maxTime = minTime;
    }

    private Vector3 CastRayForward() {
        Vector3 forwardDirection = transform.forward;
        if (Physics.Raycast(transform.position, forwardDirection, out RaycastHit hitInfo, maxWallSearchDist, layerMask)) {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            return hitInfo.point;
        }
        else {
            Debug.DrawRay(transform.position, forwardDirection * maxWallSearchDist, Color.green);

        }
        return Vector3.zero;
    }

    private Vector3 CastRayAtAngle(float angle) {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        Vector3 rotatedRight = rotation * Vector3.forward;
        Vector3 rayDir = transform.position + rotatedRight * 1f - transform.position;

        if (Physics.Raycast(transform.position, rayDir, out RaycastHit hitInfo, maxWallSearchDist, layerMask)) {
            Debug.DrawLine(transform.position, hitInfo.point, Color.yellow);
            return hitInfo.point;
        }
        else {
            Debug.DrawRay(transform.position, rayDir * maxWallSearchDist, Color.magenta);
        }
        return Vector3.zero;
    }

    public void EnterChokehold() {
        var enemy = FindObjectOfType<Enemy>();
        enemy.enabled = false;
        agent = enemy.GetComponent<NavMeshAgent>();
        agent.enabled = true;

        FindObjectOfType<MouseLook>().enabled = false;
        FindObjectOfType<NormalMove>().enabled = false;
        FindObjectOfType<Gun>().enabled = false;

        var vCams = FindObjectsByType<CinemachineVirtualCamera>(FindObjectsSortMode.InstanceID);
        for (int i = 0; i < vCams.Length; i++) {
            vCams[i].gameObject.SetActive(false);
        }

        virtualChokeCamera.gameObject.SetActive(true);

        grip = maxGrip;
        chokeTime = startTime;

        animator.SetBool("IsBeingStrangled", true);

        inChokehold = true;

        chockingSource.Play();

        timeInChokeHold = 0;

        gameObject.tag = "Player";
    }

    public void ExitChokehold() {
        maxTime = maxTime - timeLostOnEachAttack;
        if (maxTime < minTime) maxTime = minTime;

        chokeTime = 0;
        inChokehold = false;
        var enemy = FindObjectOfType<Enemy>();
        enemy.enabled = true;
        enemy.GetComponent<NavMeshAgent>().enabled = true;

        FindObjectOfType<MouseLook>().enabled = true;
        FindObjectOfType<NormalMove>().enabled = true;
        FindObjectOfType<Gun>().enabled = true;

        var vCams = FindObjectsByType<CinemachineVirtualCamera>(FindObjectsSortMode.InstanceID);
        for (int i = 0; i < vCams.Length; i++) {
            vCams[i].gameObject.SetActive(false);
        }

        timeInChokeHold = 0;

        onChokeHoldEnd.Invoke();

        animator.SetBool("IsBeingStrangled", false);

        gameObject.tag = "Enemy";

        chockingSource.Stop();

        NormalMove move = FindObjectOfType<NormalMove>();
        var playerController = move.GetComponent<CharacterController>();
        playerController.enabled = false;

        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position - transform.forward * 4f, out hit, 20f, NavMesh.AllAreas);

        playerController.transform.position = hit.position;
        playerController.enabled = true;

        //TODO enable the Players Camera
    }
}
