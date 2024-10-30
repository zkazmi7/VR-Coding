using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScriptt : MonoBehaviour
{
    public Camera viewCamera;
    public GameObject cursorPrefab;
    public float maxCursorDistance = 30;
    private GameObject cursorInstance;
    // Start is called before the first frame update
    void Start()
    {
        cursorInstance = Instantiate(cursorPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        // Create a gaze ray pointing forward from the camera
        Ray ray = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // If the ray hits something, set the position to the hit point
            // and rotate based on the normal vector of the hit
            cursorInstance.transform.position = hit.point;
            cursorInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
        else
        {
            // If the ray doesn't hit anything, set the position to the maxCursorDistance
            // and rotate to point away from the camera
            cursorInstance.transform.position = ray.origin + ray.direction.normalized * maxCursorDistance;
            cursorInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, -ray.direction);
        }
    }
}
