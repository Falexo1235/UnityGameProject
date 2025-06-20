using UnityEngine;

public interface IUsableItem
{
    void UseStart();//Called when the button is first pressed
    void UseHold(); //Called while the button is held
    void UseEnd(); //Called when the button is depressed
} 