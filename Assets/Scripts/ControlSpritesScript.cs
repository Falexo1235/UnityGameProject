using UnityEngine;
public class ControlSpritesScript : MonoBehaviour
{
    public GameObject hand;
    public KeyCode toggleKey = KeyCode.H;
    private Camera mainCamera;
    private Color defaultColor = Color.white;
    private Color pressedColor = Color.orange;
    private Color grabbedColor = Color.green;
    private Color currentColor;
    private SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentColor = defaultColor;
        sr.color = currentColor;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraRotation = mainCamera.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, 0, cameraRotation.z);
        transform.position = hand.transform.position-Vector3.forward/2;
        if (Input.GetKeyDown(toggleKey))
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        }
    }

    public void SetColor(int color)
    {
        switch (color)
        {
            case 0:
                sr.color = defaultColor;
                break;
            case 1:
                sr.color = pressedColor;
                break;
            case 2:
                sr.color = grabbedColor;
                break;
        }
    }
}
