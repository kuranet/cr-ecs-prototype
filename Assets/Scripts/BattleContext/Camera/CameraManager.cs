using UnityEngine;

public class CameraManager 
{
    public static Vector3 GetCameraOrientedPos()
    {
        Camera cam = Camera.main;
        Vector3 mouse = Input.mousePosition;

        float groundY = 0f;
        Vector3 camPos = cam.transform.position;
        Vector3 camForward = cam.transform.forward;

        float t = (groundY - camPos.y) / camForward.y;
        if (t < 0) t = 0f;

        mouse.z = t;
        return cam.ScreenToWorldPoint(mouse);
    }
}
