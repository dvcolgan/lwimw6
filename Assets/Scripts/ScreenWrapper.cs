using UnityEngine;
using System.Collections;

public class ScreenWrapper : MonoBehaviour
{

    private Camera cam;

    void Start ()
    {
        cam = Camera.main;
    }

    void OnBecameInvisible ()
    {
        Vector3 pos = transform.position;

        if (cam != null) {
            Vector3 viewportPosition = cam.WorldToViewportPoint (transform.position);
            if (viewportPosition.x > 1 || viewportPosition.x < 0 || viewportPosition.y > 1 || viewportPosition.y < 0) {

                if (viewportPosition.x > 1) {
                    pos.x = -pos.x + 1;
                }
                if (viewportPosition.x < 0) {
                    pos.x = -pos.x - 1;
                }
                if (viewportPosition.y > 1) {
                    pos.y = -pos.y + 1;
                }
                if (viewportPosition.y < 0) {
                    pos.y = -pos.y - 1;
                }
            }
            transform.position = pos;
        }
    }
}
