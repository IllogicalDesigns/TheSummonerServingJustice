using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceMarker : MonoBehaviour
{
    public Transform target;
    [SerializeField] Image img;
    [SerializeField] Vector3 offset = Vector3.zero;
    Camera cam;

    // Start is called before the first frame update
    IEnumerator Start() {
        yield return new WaitForSeconds(0.1f);

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        if (cam == null || target == null) return;
        else
            PositionImage();
    }

    private void PositionImage() {
        // Giving limits to the icon so it sticks on the screen
        // Below calculations witht the assumption that the icon anchor point is in the middle
        // Minimum X position: half of the icon width
        float minX = img.GetPixelAdjustedRect().width / 2;
        // Maximum X position: screen width - half of the icon width
        float maxX = Screen.width - minX;

        // Minimum Y position: half of the height
        float minY = img.GetPixelAdjustedRect().height / 2;
        // Maximum Y position: screen height - half of the icon height
        float maxY = Screen.height - minY;

        // Temporary variable to store the converted position from 3D world point to 2D screen point
        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);

        // Check if the target is behind us, to only show the icon once the target is in front
        if (Vector3.Dot((target.position - transform.position), transform.forward) < 0) {
            // Check if the target is on the left side of the screen
            if (pos.x < Screen.width / 2) {
                // Place it on the right (Since it's behind the player, it's the opposite)
                pos.x = maxX;
            }
            else {
                // Place it on the left side
                pos.x = minX;
            }
        }

        // Limit the X and Y positions
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // Update the marker's position
        img.transform.position = pos;
    }
}
