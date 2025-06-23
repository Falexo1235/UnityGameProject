using UnityEngine;

public class SmoothCameraScript : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 cameraOffset = -Vector3.forward;
    private bool rotating = false;
    private Quaternion targetRotation;
    //For the missile mazes
    [HideInInspector]
    public float targetZoom;
    private Camera selfCamera;
    private void Start()
    {
        selfCamera = gameObject.GetComponent<Camera>();
        targetZoom = selfCamera.orthographicSize;
    }
    void Update()
    {
        Vector3 targetPosition = target.position + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        selfCamera.orthographicSize = Mathf.Lerp(selfCamera.orthographicSize, targetZoom, Time.deltaTime * 5f);

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
