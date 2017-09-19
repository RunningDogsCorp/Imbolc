using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateManager : MonoBehaviour
{
    public Image panelSample;
    public int panelNum;
    public Transform parent;
    public float speed;
    public float moveTime;
    public List<GameObject> panels;

    private static UpdateManager sInstance;
    public static UpdateManager getinstance
    {
        get
        {
            if(sInstance == null)
            {
                GameObject newObject = new GameObject("UpdateManager");
                sInstance = newObject.AddComponent<UpdateManager>();
            }

            return sInstance;
        }
    }

    private void Awake()
    {
        CreateUpdatePanel();
    }

    void CreateUpdatePanel()
    {
        for(int i = 0; i < panelNum; ++i)
        {
            GameObject newObject = Instantiate(panelSample.gameObject, parent);
            panels.Add(newObject);
        }
    }
}
