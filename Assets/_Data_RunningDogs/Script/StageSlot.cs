using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSlot : MonoBehaviour
{
    public int index;

    public void ClickToSlots()
    {
        if (StageManager.getInstance.currentIndex != index) StageManager.getInstance.currentIndex = index;
        else
            StageManager.getInstance.ready = true;
    }
}
