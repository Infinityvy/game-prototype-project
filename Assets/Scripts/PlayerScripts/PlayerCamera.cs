using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // public:
    public new Camera camera;
    public float zoomSpeed = 1f;

    // private:
    private readonly float minCameraZoom = 1.0f;
    private readonly float maxCameraZoom = 17.0f;
    private readonly float defaultCameraZoom = 5.0f;

    void Start()
    {
        
    }

    void Update()
    {
        float scrollAxis = -Input.GetAxis("Mouse ScrollWheel");

        if (scrollAxis != 0)
        {
            float newZoom = camera.orthographicSize + scrollAxis * zoomSpeed * Time.deltaTime * 1000;
            newZoom = Mathf.Clamp(newZoom, minCameraZoom, maxCameraZoom);
            camera.orthographicSize = newZoom;
        }
        if(Input.GetKeyDown(GameInputs.keys["Reset Camera"]))
        {
            camera.orthographicSize = defaultCameraZoom;
        }
    }
}
