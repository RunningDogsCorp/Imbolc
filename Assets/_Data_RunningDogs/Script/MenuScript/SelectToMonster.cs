using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectToMonster : MonoBehaviour
{
    public Vector2 center;
    public GameObject panel;
    public GameObject iconSample;
    public GameObject selectButton;
    public float speed;
    public float scaleSpeed;

    public float rekoningTime;
    public float changeTime;
    public float changeLimit;

    public float moveTime;
    public bool move;
    public bool rekoningToRandomIndex;

    

    private void Start()
    {
        changeLimit = changeTime;
    }

    public void RekoningMoveSpeed()
    {
        if (EnemyManager.getInstance.icons[EnemyManager.getInstance.index].transform.position.x > center.x)
            speed = (EnemyManager.getInstance.icons[EnemyManager.getInstance.index].transform.position.x - center.x)
                / (moveTime * 60.0f);
        else if (EnemyManager.getInstance.icons[EnemyManager.getInstance.index].transform.position.x < center.x)
            speed = (center.x - EnemyManager.getInstance.icons[EnemyManager.getInstance.index].transform.position.x)
                / (moveTime * 60.0f);

        scaleSpeed = 150.0f / (moveTime * 60);
    }
    
    public void SortMonIcons()
    {
        for(int i =0; i < EnemyManager.getInstance.icons.Count; ++i)
        {
            if (i > EnemyManager.getInstance.index) EnemyManager.getInstance.icons[i].transform.position =
                     new Vector2(960.0f + ((i - EnemyManager.getInstance.index) * 480.0f), center.y);
            else if (i < EnemyManager.getInstance.index) EnemyManager.getInstance.icons[i].transform.position =
                     new Vector2(960.0f - ((EnemyManager.getInstance.index -i) * 480.0f), center.y);
            else EnemyManager.getInstance.icons[i].transform.position = center;
        }
    }
    public IEnumerator MoveControll()
    {
        if (EnemyManager.getInstance.icons[EnemyManager.getInstance.index].transform.position.x > center.x)
        {
            for (int i = 0; i < EnemyManager.getInstance.icons.Count; ++i)
            {
                EnemyManager.getInstance.icons[i].transform.Translate(-speed, 0, 0);
            }

            if (EnemyManager.getInstance.icons[EnemyManager.getInstance.index].transform.position.x - center.x <= 0.02f)
            {
                EnemyManager.getInstance.icons[EnemyManager.getInstance.index].transform.position = center;
                SortMonIcons();
                move = false;
                selectButton.SetActive(true);
            }
            else selectButton.SetActive(false);
        }
        else if(EnemyManager.getInstance.icons[EnemyManager.getInstance.index].transform.position.x < center.x)
        {
            for(int i =0; i < EnemyManager.getInstance.icons.Count; ++i)
            {
                EnemyManager.getInstance.icons[i].transform.Translate(speed, 0, 0);
            }

            if(center.x - EnemyManager.getInstance.icons[EnemyManager.getInstance.index].transform.position.x <= 0.02f)
            {
                EnemyManager.getInstance.icons[EnemyManager.getInstance.index].transform.position = center;
                SortMonIcons();
                move = false;
                selectButton.SetActive(true);
            }
            else selectButton.SetActive(false);
        }

        yield return null;
        
    }
    public IEnumerator ScaleControll()
    {
        for(int i =0; i<EnemyManager.getInstance.icons.Count; ++i)
        {
            Image temp = EnemyManager.getInstance.icons[i].GetComponent<Image>();
            if (EnemyManager.getInstance.index == i)
            {
                if (temp.rectTransform.rect.width < 350.0f)
                {
                    float width = temp.rectTransform.rect.width;
                    float height = temp.rectTransform.rect.height;

                    width += scaleSpeed;
                    height += scaleSpeed;

                    temp.rectTransform.sizeDelta = new Vector2(width, height);
                }
                else temp.rectTransform.sizeDelta = new Vector2(350.0f, 350.0f);
            }
            else
            {
                if (temp.rectTransform.rect.width > 250.0f)
                {
                    float width = temp.rectTransform.rect.width;
                    float height = temp.rectTransform.rect.height;

                    width -= scaleSpeed;
                    height -= scaleSpeed;

                    temp.rectTransform.sizeDelta = new Vector2(width, height);
                }
                else temp.rectTransform.sizeDelta = new Vector2(250.0f, 250.0f);
            }
        }
        yield return null;
    }
    public IEnumerator RandomMonsterSelect()
    {
        rekoningTime += Time.deltaTime;
        if (changeTime > 0) changeTime -= Time.deltaTime;
        else
        {
            int randomIndex = Random.Range(0, EnemyManager.getInstance.icons.Count - 1);

            Image temp = EnemyManager.getInstance.icons[EnemyManager.getInstance.icons.Count - 1].GetComponent<Image>();
            temp.sprite = EnemyManager.getInstance.monIcons[randomIndex].sprite;
            changeTime = changeLimit;
            Debug.Log("RandomIndex : " + randomIndex);
        }

        if (EnemyManager.getInstance.index != EnemyManager.getInstance.icons.Count - 1) 
        {
            rekoningTime = 0;
            rekoningToRandomIndex = false;
        }

        yield return null;
    }

    public void WakeUpToSelection()
    {
        panel.SetActive(true);
        SortMonIcons();
    }
    public void CloseToSelection()
    {
        panel.SetActive(false);
    }

    public void CreateToIcons()
    {
        for(int i =0; i < EnemyManager.getInstance.monIcons.Length; ++i)
        {
            GameObject newObject = Instantiate(iconSample, panel.transform);
            Image img = newObject.GetComponent<Image>();

            img.sprite = EnemyManager.getInstance.monIcons[i].sprite;
            EnemyManager.getInstance.icons.Add(newObject);
        }

        GameObject lastObject = Instantiate(iconSample, panel.transform);
        EnemyManager.getInstance.icons.Add(lastObject);

    }
}
