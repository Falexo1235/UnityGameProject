using UnityEngine;

public class TestProjectileScript : MonoBehaviour
{

    public void changeColor()
    {
        GetComponent<Renderer>().material.color = new Color(255, 0, 0);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
