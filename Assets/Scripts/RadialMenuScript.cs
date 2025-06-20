using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RadialMenu : MonoBehaviour
{
    public GameObject menuUI;
    public int slotCount = 8;
    public Image[] slotImages; //Image components are used as slots, they are created in ui/ menu
    public int selectedSlot = -1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menuUI.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Tab))
        {
            UpdateSelection();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            menuUI.SetActive(false);
            if (selectedSlot != -1)
            {
                SelectItem(selectedSlot);
            }
        }
    }

    void UpdateSelection()
    {
        Vector2 mousePos = Input.mousePosition - menuUI.transform.position;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        angle = 360f - angle - (360f / slotCount) + 180f;
        if (angle >= 360f) angle -= 360f;
        int newSelected = Mathf.FloorToInt(angle / (360f / slotCount));
        if (newSelected != selectedSlot)
        {
            selectedSlot = newSelected;
            HighlightSlot(selectedSlot);
        }
    }

    void HighlightSlot(int slot)
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].color = (i == slot) ? Color.gray : Color.white;
        }
    }

    void SelectItem(int slot)
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.EquipItem(slot);
        }
        else
        {
            Debug.LogWarning("No Inventory instance found");
        }
    }
}