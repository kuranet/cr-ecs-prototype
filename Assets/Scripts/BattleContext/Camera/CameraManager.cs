using UnityEngine;

public class CameraManager 
{
    public static Vector3 GetCameraOrientedPos()
    {
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        float groundY = 0f;
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));

        if (groundPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }
}
