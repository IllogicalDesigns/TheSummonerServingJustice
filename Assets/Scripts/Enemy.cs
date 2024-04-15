using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] UnityEvent lose;
    [SerializeField] GameObject player;
    VisionSensor visionSensor;
    NavMeshAgent agent;

    [SerializeField] public float awareness;
    [SerializeField] public float awarenessThreshold = 2f;

    [Space]
    [SerializeField] AnimationCurve speedOverDistanceCurve;

    [Space]
    [SerializeField] float number = 5f;

    public bool isAware;
    bool isPaused;
    bool slowed;
    [SerializeField] float slowedTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        visionSensor = GetComponent<VisionSensor>();
    }

    //private void OnEnable() {
    //    GameManager.instance.OnPlayerPaused += OnPlayerPaused;
    //}

    //private void OnDisable() {
    //    GameManager.instance.OnPlayerPaused -= OnPlayerPaused;
    //}

    //public void OnPlayerPaused(bool paused) {
    //    agent.isStopped = !paused;
    //    agent.enabled = !paused;
    //    isPaused = paused;
    //}

    public void MaintainDistance() {
        float currentDistance = Vector3.Distance(transform.position, player.transform.position);

        float desiredSpeed = speedOverDistanceCurve.Evaluate(currentDistance);

        // Set the NavMeshAgent's speed to the desired speed
        agent.speed = desiredSpeed;
    }

    public void SlowImpact() {
        if(!isAware) isAware = true;
        slowed = true;
        Invoke(nameof(unSlowed), slowedTime);
    }

    public void unSlowed() {
        slowed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(slowed) {
            agent.SetDestination(transform.position);
            return;
        }

        if (isPaused) return;

        MaintainDistance();

        if (!isAware) {
            if(player) {
                AugmentAwareness();
            }

            if (awareness >= awarenessThreshold) isAware = true;

            return;
        }

        agent.SetDestination(target.position);  

        if(Vector3.Distance(transform.position, target.position) < 2f) {
            lose.Invoke();
        }
    }

    private void AugmentAwareness() {
        float visionMulti = visionSensor.CanWeSeeTarget(player);
        if (visionMulti > 0) {
            awareness += visionMulti * Time.deltaTime;
        }
        else {
            if (awareness > 0) awareness -= Time.deltaTime;
        }
    }
}
