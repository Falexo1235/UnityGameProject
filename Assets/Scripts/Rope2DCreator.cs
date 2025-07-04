using UnityEngine;
using NaughtyAttributes;
public class Rope2DCreator : MonoBehaviour
{
    [SerializeField, Range(2, 50)]
    int segmentsCount = 2;

    public Transform pointA;
    public Transform pointB;
    public HingeJoint2D hingePrefab;

    [HideInInspector]
    public Transform[] segments;
    
    [Button]
    void GenerateRope()
    {
        segments = new Transform[segmentsCount];
        for (int i = 0; i < segmentsCount; i++)
        {
            var currJoint = Instantiate(hingePrefab, GetSegmentPosition(i), Quaternion.identity, this.transform);
            segments[i] = currJoint.transform;
            if (i > 0)
            {
                int prevIndex = i - 1;
                currJoint.connectedBody = segments[prevIndex].GetComponent<Rigidbody2D>();
            }
        }
    }
    [Button]
    void DeleteSegments()
    {
        if (transform.childCount > 0)
        {
            for(int i = transform.childCount - 1; i>=0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        segments = null;
    }
    Vector2 GetSegmentPosition (int segmentIndex)
    {
        Vector2 posA = pointA.position;
        Vector2 posB = pointB.position;
        float fraction = 1f / (float)segmentsCount;
        return Vector2.Lerp(posA, posB, fraction * segmentIndex);
    }
    private void OnDrawGizmos()
    {
        if (pointA == null || pointB == null) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < segmentsCount; i++)
        {
            Vector2 posAtIndx = GetSegmentPosition(i);
            Gizmos.DrawSphere(posAtIndx, 0.1f);
        }

    }
}
