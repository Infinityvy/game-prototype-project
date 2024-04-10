using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // public:
    public new Camera camera;
    public float zoomSpeed = 1f;

    // private:
    private readonly float minCameraZoom = -5.0f;
    private readonly float maxCameraZoom = -60.0f;
    private readonly float defaultCameraZoom = -20.0f;

    void Start()
    {
        
    }

    void Update()
    {
        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");

        if (scrollAxis != 0)
        {
            //float newZoom = camera.orthographicSize + scrollAxis * zoomSpeed * Time.deltaTime * 1000;
            float newZoom = camera.transform.localPosition.z + scrollAxis * zoomSpeed * Time.deltaTime * 1000;
            newZoom = Mathf.Clamp(newZoom, maxCameraZoom, minCameraZoom);
            //camera.orthographicSize = newZoom;
            camera.transform.localPosition = new Vector3(0, 0, newZoom);
        }
        if(Input.GetKeyDown(GameInputs.keys["Reset Camera"]))
        {
            //camera.orthographicSize = defaultCameraZoom;
            camera.transform.localPosition = new Vector3(0, 0, defaultCameraZoom);
        }
    }
}
