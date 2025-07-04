using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    private float t = 0f;

    void Update()
    {
        if (pointA == null || pointB == null) return;
        t += Time.deltaTime * moveSpeed;
        float lerpValue = Mathf.PingPong(t, 1f);
        transform.position = Vector3.Lerp(pointA.position, pointB.position, lerpValue);
    }
}