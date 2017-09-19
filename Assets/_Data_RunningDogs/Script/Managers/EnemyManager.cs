using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public Image[] monIcons;
    public int index;

    public List<GameObject> icons;

    private static EnemyManager sInstance;
    public static EnemyManager getInstance
    {
        get
        {
            if (sInstance == null)
            {
                GameObject newGameObject = new GameObject("EneyManager");
                sInstance = newGameObject.AddComponent<EnemyManager>();
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
    }
}
