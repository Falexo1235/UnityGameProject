using UnityEngine;
using UnityEngine.UI;
public class ControlSpritesScript : MonoBehaviour
{
    public GameObject hand;
    public KeyCode toggleKey = KeyCode.H;
    private Camera mainCamera;
    private Color defaultColor = Color.white;
    private Color pressedColor = Color.orange;
    private Color grabbedColor = Color.green;
    private Color currentColor;
    private Image imageComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        imageComponent = GetComponent<Image>();
        currentColor = defaultColor;
        imageComponent.color = currentColor;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraRotation = mainCamera.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, 0, cameraRotation.z);
        transform.position = Camera.main.WorldToScreenPoint(hand.transform.position);
        if (Input.GetKeyDown(toggleKey))
        {
            imageComponent.enabled = !imageComponent.enabled;
        }
    }

    public void SetColor(int color)
    {
        switch (color)
        {
            case 0:
                imageComponent.color = defaultColor;
                break;
            case 1:
                imageComponent.color = pressedColor;
                break;
            case 2:
                imageComponent.color = grabbedColor;
                break;
        }
    }
}
