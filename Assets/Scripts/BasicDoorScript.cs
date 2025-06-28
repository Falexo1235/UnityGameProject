using UnityEngine;

public class BasicDoorScript : MonoBehaviour
{
    public Vector3 moveCoordinates;
    private Transform currentTransform;
    private void Start()
    {
        currentTransform = gameObject.transform;
    }
    public void OpenDoor()
    {
        currentTransform.position += moveCoordinates;
    }
    private void Update()
    {
        gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, currentTransform.position, 3f);
    }
}
