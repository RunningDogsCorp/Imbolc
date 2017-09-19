using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public Image slotSample;
    public int currentIndex;
    public int stageNum;
    public bool ready;
    public Transform parent;
    public List<GameObject> slots;
    public GameObject stageControllPanel;

    private static StageManager sInstance;
    public static StageManager getInstance
    {
        get
        {
            if(sInstance == null)
            {
                GameObject newObject = new GameObject("StageManager");
                sInstance = newObject.AddComponent<StageManager>();
            }

            return sInstance;
        }
    }

    private void Awake()
    {
        if (sInstance == null)
        {
            sInstance = this;
        }
        else Destroy(this.gameObject);

        CreateStageIcon();
        SortIcons();
    }

    void CreateStageIcon()
    {
        for(int i =0; i < stageNum; ++i)
        {
            GameObject newObject = Instantiate(slotSample.gameObject, parent);
            StageSlot slot = newObject.GetComponent<StageSlot>();
            slot.index = i;
            slots.Add(newObject);
        }
    }
    
    public void SortIcons()
    {
        for(int i =0; i < slots.Count; ++i)
        {
            slots[i].transform.position = new Vector2(1800.0f - (i * 80.0f), 765.0f);

            if(i == currentIndex)
            {
                Image img = slots[i].GetComponent<Image>();

                img.rectTransform.sizeDelta = new Vector2(70.0f, 70.0f);
            }
            else
            {
                Image img = slots[i].GetComponent<Image>();

                img.rectTransform.sizeDelta = new Vector2(50.0f, 50.0f);
            }
        }
    }

    public void GoToStage()
    {
        if(ready) SceneManager.LoadScene("InGameScene");
    }

    public void WakeUpControllPanel()   { stageControllPanel.SetActive(true); }
    public void CloseControllPanel()    { stageControllPanel.SetActive(false); }
}
