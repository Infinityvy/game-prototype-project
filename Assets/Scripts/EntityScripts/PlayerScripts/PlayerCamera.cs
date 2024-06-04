using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // public:
    public static PlayerCamera instance;

    public new Camera camera;
    public float zoomSpeed = 1f;

    // private:
    private readonly float minCameraZoom = -5.8f;
    private readonly float maxCameraZoom = -35.55f;
    private readonly float defaultCameraZoom = -25.0f;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");

        if (scrollAxis != 0)
        {
            float newZoom = camera.transform.localPosition.z + scrollAxis * zoomSpeed * Time.deltaTime * 1000;
            newZoom = Mathf.Clamp(newZoom, maxCameraZoom, minCameraZoom);
            camera.transform.localPosition = new Vector3(0, 0, newZoom);
        }
        if(Input.GetKeyDown(GameInputs.keys["Reset Camera"]))
        {
            camera.transform.localPosition = new Vector3(0, 0, defaultCameraZoom);
        }
    }

    /// <summary>
    /// Returns on which side of the camera view the given object is located. -1 for left, 1 for right.
    /// </summary>
    /// <param name="objectPosition"></param>
    /// <returns></returns>
    public int getSideOfCamera(Vector3 objectPosition)
    {
        Vector3 rightVector = Vector3.Cross(camera.transform.forward, Vector3.up).normalized;
        Vector3 camToObject = objectPosition - camera.transform.position;
        float dotProduct = Vector3.Dot(rightVector, camToObject);

        return dotProduct < 0 ? 1 : -1;
    }
}
