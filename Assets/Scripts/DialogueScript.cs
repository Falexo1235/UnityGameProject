using UnityEngine;
using System.Collections;

public class DialogueScript : MonoBehaviour
{
    public GameObject uiDialogueObject;
    public float scaleSpeed = 2f;
    public float displayTime = 5f;
    public bool startHidden = true;
    
    private Vector3 originalUIScale;
    private bool isScaling = false;
    private bool isDisplaying = false;
    private Camera mainCamera;
    private RectTransform uiRectTransform;
    
    void Start()
    {
        mainCamera = Camera.main;
        if (uiDialogueObject != null)
        {
            uiRectTransform = uiDialogueObject.GetComponent<RectTransform>();
            originalUIScale = uiDialogueObject.transform.localScale;
            if (startHidden)
            {
                uiDialogueObject.transform.localScale = Vector3.zero;
            }
        }
        else
        {
            Debug.LogError("UI Dialogue Object not assigned!");
        }
    }
    
    void Update()
    {
        if (uiRectTransform != null && mainCamera != null)
        {
            UpdateUIPosition();
        }
    }
    
    void UpdateUIPosition()
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiRectTransform.parent as RectTransform, 
            screenPos, 
            null,
            out localPos
        );
        
        uiRectTransform.anchoredPosition = localPos;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerTag player = other.GetComponent<PlayerTag>();
        if (player != null && !isScaling && !isDisplaying && uiDialogueObject != null)
        {
            StartCoroutine(ShowDialogue());
        }
    }
    
    IEnumerator ShowDialogue()
    {
        isScaling = true;

        while (Vector3.Distance(uiDialogueObject.transform.localScale, originalUIScale) > 0.01f)
        {
            uiDialogueObject.transform.localScale = Vector3.Lerp(uiDialogueObject.transform.localScale, originalUIScale, Time.deltaTime * scaleSpeed);
            yield return null;
        }

        uiDialogueObject.transform.localScale = originalUIScale;
        isScaling = false;
        isDisplaying = true;

        yield return new WaitForSeconds(displayTime);

        while (uiDialogueObject.transform.localScale.x > 0.01f)
        {
            uiDialogueObject.transform.localScale = Vector3.Lerp(uiDialogueObject.transform.localScale, Vector3.zero, Time.deltaTime * scaleSpeed);
            yield return null;
        }

        Destroy(uiDialogueObject);
    }
}