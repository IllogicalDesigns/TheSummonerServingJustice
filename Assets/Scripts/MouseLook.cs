using System.Collections;
using UnityEngine;

public class MouseLook : MonoBehaviour {
    public float sensitivityX = 100f; // Sensitivity for X axis rotation
    public float sensitivityY = 100f; // Sensitivity for Y axis rotation
    public float minClampAngle = -90f; // Minimum clamp angle for Y axis rotation
    public float maxClampAngle = 90f; // Maximum clamp angle for Y axis rotation

    private float currentXRotation; // Current X rotation stored as a property
    [SerializeField] Transform camTransform; // Reference to the camera transform

    [SerializeField] bool isInverted = true;
    bool isPaused;

    private void Start() {
        // Get the camera transform reference
        //camTransform = Camera.main.transform;
        Utility.LockAndHideCursor();
    }

    //private void OnEnable() {
    //    GameManager.instance.OnPlayerPaused += OnPlayerPaused;
    //}

    //private void OnDisable() {
    //    GameManager.instance.OnPlayerPaused -= OnPlayerPaused;
    //}

    //public void OnPlayerPaused(bool paused) {
    //    isPaused = paused;
    //}

    void Update() {
        if (isPaused) return;

        // Handle mouse scroll events
        if (Input.mouseScrollDelta.y != 0) {
            sensitivityX += Input.mouseScrollDelta.y;
            sensitivityY += Input.mouseScrollDelta.y;
        }
            

        // Rotate around the X axis based on Mouse Y input
        float yRotation = (isInverted?-1:1) * Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
        currentXRotation -= yRotation; // Subtract because we are rotating around the negative Z axis
        currentXRotation = Mathf.Clamp(currentXRotation, minClampAngle, maxClampAngle);

        // Apply the X axis rotation to the camera transform
        camTransform.localRotation = Quaternion.Euler(-currentXRotation, 0f, 0f);

        // Rotate around the Y axis based on Mouse X input
        float xRotation = Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;

        // Apply the Y axis rotation to the player transform
        transform.localRotation *= Quaternion.Euler(0f, xRotation, 0f);
    }
}