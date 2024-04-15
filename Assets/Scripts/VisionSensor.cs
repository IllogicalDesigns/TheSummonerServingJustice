using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    [SerializeField] bool isDebug = true;
    [SerializeField] float visionRange = 30f;

    [Space]
    [SerializeField] LayerMask detectionMask = 1 << 8;

    [Space]
    [SerializeField] private CoffinDefinition primaryCoffin, secondaryCoffin;
    [SerializeField] private ShoulderZoneDefinition shoulderZone;

    [Space]
    [SerializeField] private float peripheryMulti = 0.5f;
    [SerializeField] private float primaryMulti = 1f;

    bool isValid(GameObject candidateGameObject) {
        //Don't detect ourseleves
        if (candidateGameObject == gameObject) return false;

        if (isDebug) Debug.DrawLine(transform.position, candidateGameObject.transform.position, Color.gray);

        return true;
    }

    bool isInRange(GameObject candidateGameObject) {
        if (Vector3.Distance(candidateGameObject.transform.position, transform.position) < visionRange) {

            if (isDebug) Debug.DrawLine(transform.position, candidateGameObject.transform.position, Color.white);

            return true;
        }

        return false;
    }

    bool inPrimaryCoffin(GameObject candidateGameObject) {
        var candidatePosition = candidateGameObject.transform.position;

        return CheckCoffin(primaryCoffin.farWidth, primaryCoffin.farDist, primaryCoffin.midWidth, primaryCoffin.midDist, primaryCoffin.closeWidth, primaryCoffin.closeDist, candidatePosition);
    }

    bool inSecondaryCoffin(GameObject candidateGameObject) {
        var candidatePosition = candidateGameObject.transform.position;

        secondaryCoffin.farDist = visionRange;

        return CheckCoffin(secondaryCoffin.farWidth, secondaryCoffin.farDist, secondaryCoffin.midWidth, secondaryCoffin.midDist, secondaryCoffin.closeWidth, secondaryCoffin.closeDist, candidatePosition);
    }

    private bool CheckCoffin(float farWidth, float farDist, float midWidth, float midDist, float closeWidth, float closeDist, Vector3 candidatePosition) {
        Vector3 farLeft = (transform.position + transform.forward * farDist) + transform.right * -farWidth;
        Vector3 farRight = transform.position + transform.forward * farDist + transform.right * farWidth;

        Vector3 midLeft = (transform.position + transform.forward * midDist) + transform.right * -midWidth;
        Vector3 midRight = transform.position + transform.forward * midDist + transform.right * midWidth;

        Vector3 closeLeft = (transform.position + transform.forward * closeDist) + transform.right * -closeWidth;
        Vector3 closeRight = transform.position + transform.forward * closeDist + transform.right * closeWidth;

        bool isInCoffin = false;
        if (CheckTrapezoid(closeLeft, closeRight, midLeft, midRight, candidatePosition))
            isInCoffin = true;

        if (CheckTrapezoid(midLeft, midRight, farLeft, farRight, candidatePosition))
            isInCoffin = true;

        return isInCoffin;
    }

    private bool CheckTrapezoid(Vector3 closeLeft, Vector3 closeRight, Vector3 midLeft, Vector3 midRight, Vector3 candidatePosition) {
        bool isInTrapezoid = false;
        if (CheckTriangle(closeLeft, closeRight, midLeft, candidatePosition))
            isInTrapezoid = true;

        if (CheckTriangle(midLeft, midRight, closeRight, candidatePosition))
            isInTrapezoid = true;

        return isInTrapezoid;
    }

    private bool CheckTriangle(Vector3 a, Vector3 b, Vector3 c, Vector3 p) {
        // Compute vectors
        var v0 = c - a;
        var v1 = b - a;
        var v2 = p - a;

        // Compute dot products
        var dot00 = Vector3.Dot(v0, v0);
        var dot01 = Vector3.Dot(v0, v1);
        var dot02 = Vector3.Dot(v0, v2);
        var dot11 = Vector3.Dot(v1, v1);
        var dot12 = Vector3.Dot(v1, v2);

        // Compute barycentric coordinates
        var invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        var u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        var v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        // Check if point is in triangle
        bool isInTri = (u >= 0) && (v >= 0) && (u + v < 1);

        if (isDebug) {
            Color clr = !isInTri ? Color.red : Color.green;
            Debug.DrawLine(a, b, clr);
            Debug.DrawLine(b, c, clr);
            Debug.DrawLine(c, a, clr);
        }

        return isInTri;
    }

    private bool CheckShoulderShape(float closeWidth, float closeDist, float farWidth, float farDist, Vector3 candidatePosition) {
        var closeVec1 = (transform.position + transform.forward * closeDist) + transform.right * -closeWidth;
        var closeVec2 = (transform.position + transform.forward * closeDist) + transform.right * (-closeWidth * 0.5f);

        var farVec1 = (transform.position + transform.forward * farDist) + transform.right * -farWidth;
        var farVec2 = (transform.position + transform.forward * farDist) + transform.right * (-farWidth * 0.5f);

        bool pointInSpace = CheckTrapezoid(closeVec1, closeVec2, farVec1, farVec2, candidatePosition);
        if (!pointInSpace) pointInSpace = CheckTriangle(closeVec2, farVec2, transform.position, candidatePosition);

        return pointInSpace;
    }

    bool inOverShoulderZone(GameObject candidateGameObject) {
        var candidatePosition = candidateGameObject.transform.position;

        if (Vector3.Distance(candidatePosition, transform.position) > Mathf.Abs(shoulderZone.farDist) + shoulderZone.farWidth) return false;

        bool shoulder = CheckShoulderShape(shoulderZone.closeWidth, shoulderZone.closeDist, shoulderZone.farWidth, shoulderZone.farDist, candidatePosition);
        if (!shoulder) shoulder = CheckShoulderShape(-shoulderZone.closeWidth, shoulderZone.closeDist, -shoulderZone.farWidth, shoulderZone.farDist, candidatePosition);

        return shoulder;
    }

    bool hasLineOfSight(GameObject candidateGameObject, Vector3 direction) {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, visionRange, detectionMask)) {
            if (hit.collider.gameObject == candidateGameObject) {
                if (isDebug) Debug.DrawLine(transform.position, candidateGameObject.transform.position, Color.green);
                return true;
            }
            else if (isDebug) Debug.DrawLine(transform.position, candidateGameObject.transform.position, Color.yellow);
        }

        return false;
    }

    public float CanWeSeeTarget(GameObject candidateGameObject) {
        if (!isValid(candidateGameObject)) return 0f;

        if (!isInRange(candidateGameObject)) return 0f;

        Vector3 direction = candidateGameObject.transform.position - transform.position;

        bool lineOfSight = hasLineOfSight(candidateGameObject, direction);
        if (!lineOfSight) return 0f;

        bool inSecondaryVisionCoffin = inSecondaryCoffin(candidateGameObject);
        if (!inSecondaryVisionCoffin) return 0f;

        bool inPrimaryVisionCoffin = inPrimaryCoffin(candidateGameObject);
        bool inRearVisionZone = inOverShoulderZone(candidateGameObject);

        if (!inPrimaryVisionCoffin && !inRearVisionZone) return peripheryMulti;

        return primaryMulti;
    }
}

[System.Serializable]
public class CoffinDefinition {
    public float farWidth = 2.5f;
    public float farDist = 15f;

    public float midWidth = 4f;
    public float midDist = 5f;

    public float closeWidth = 1f;
    public float closeDist = 0f;
}

[System.Serializable]
public class ShoulderZoneDefinition {
    public float closeWidth = 2f;
    public float closeDist = 0f;

    public float farWidth = 2f;
    public float farDist = -1;
}
