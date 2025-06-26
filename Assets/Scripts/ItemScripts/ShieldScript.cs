using UnityEngine;

public class ShieldScript : MonoBehaviour, IUsableItem
{
    public GameObject forceField;
    public AudioClip activateSound;
    public AudioClip deactivateSound;
    //I don't know if it's even useful with customizable charge in the inventory, but I'll leave it here
    public float drainRate = 1.0f;
    
    //Public for inventory manager, needed to drain charge in the inventory itself
    [HideInInspector]
    public int itemIndex = -1;

    private AudioSource audioSource;
    void Start()
    {
        forceField.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (itemIndex == -1 || !forceField.activeSelf) return;

        float currentCharge = InventoryScript.Instance.GetCharge(itemIndex);
        currentCharge -= drainRate * Time.deltaTime;

        if (currentCharge <= 0)
        {
            currentCharge = 0;
            if (forceField.activeSelf)
            {
                forceField.SetActive(false);
                audioSource.clip = deactivateSound;
                audioSource.Play();
            }
        }
        
        InventoryScript.Instance.SetCharge(itemIndex, currentCharge);
    }

    public void UseStart()
    {
        audioSource.clip = activateSound;
        audioSource.Play();
        if (InventoryScript.Instance.GetCharge(itemIndex) > 0)
        {
            forceField.SetActive(true);
        }
    }

    public void UseHold() { }

    public void UseEnd()
    {
        if (forceField.activeSelf)
        {
            audioSource.clip = deactivateSound;
            audioSource.Play();
            forceField.SetActive(false);
        }
    }
}
