using UnityEngine;

public class LaserGunScript : MonoBehaviour, IUsableItem
{
    public GameObject laserBeam;
    public float drainRate = 1.0f;
    [HideInInspector]
    public int itemIndex = -1;
    public void UseStart()
    {
        if (InventoryScript.Instance.GetCharge(itemIndex) > 0)
        {
            laserBeam.SetActive(true);
        }
    }
    public void UseHold()
    {

    }
    public void UseEnd()
    {
        laserBeam.SetActive(false);
    }
    void Start()
    {
        laserBeam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (itemIndex == -1 || !laserBeam.activeSelf) return;

        float currentCharge = InventoryScript.Instance.GetCharge(itemIndex);
        currentCharge -= drainRate * Time.deltaTime;

        if (currentCharge <= 0)
        {
            currentCharge = 0;
            laserBeam.SetActive(false);
        }

        InventoryScript.Instance.SetCharge(itemIndex, currentCharge);
    }
}
