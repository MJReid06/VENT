using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Camera cam;

    void LateUpdate()
    {
        transform.position = cam.transform.position;
        transform.rotation = cam.transform.rotation;
    }
}
