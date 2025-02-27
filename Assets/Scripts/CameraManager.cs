using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private readonly float zoomSpeed = 5f;
    private readonly float minZoom = 5f;
    private readonly float maxZoom = 20f;
    private readonly float panSpeed = 0.5f;
    private Vector3 lastMousePosition;

    private void Update()
    {
        HandleZoom();
        if (Time.deltaTime > 0.1f) return;
        HandlePan();
        HandleKeyboardMovement();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera cam = Camera.main;
        if (cam.orthographic)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        else
        {
            cam.fieldOfView -= scroll * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
        }
    }

    private void HandlePan()
    {
        if (Input.GetMouseButtonDown(2)) lastMousePosition = Input.mousePosition;
        else if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new(-delta.x * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0);
            transform.Translate(move, Space.World);
            lastMousePosition = Input.mousePosition;
        }
    }

    private void HandleKeyboardMovement()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) move.y += 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) move.y -= 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) move.x -= 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) move.x += 1;

        transform.Translate(panSpeed * Time.deltaTime * move * 10   , Space.World);
    }
}
