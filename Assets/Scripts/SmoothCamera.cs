using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 cameraOffset = -Vector3.forward;
    private bool rotating = false;
    private Quaternion targetRotation;
    void Update()
    {
        Vector3 targetPosition = target.position + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rotating = true;
            targetRotation = target.rotation;
        }
        if (rotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 6f * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 5)
            {
                rotating = false;
            }
        }
    }
}
