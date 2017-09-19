using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;


public class ItemManager : MonoBehaviour
{
    public string fileNames;    //파일이름
    public string directoryName;//경로

    public int currentHeadgearIndex;
    public int currentWeaponIndex;
    public int currentShieldIndex;
    public int currentShopIndex;

    //위치
    public Transform headgearsPosition;
    public Transform weaponsPosition;
    public Transform shieldsPosition;
    public GameObject itemSample;

    //움직일 수 있게 할 각 아이템의 게임오브젝트 리스트
    public List<GameObject> headgearObjects;
    public List<GameObject> weaponObjects;
    public List<GameObject> shieldObjects;

    //데이터 관리하는 리스트
    public List<Items> headgears;
    public List<Items> weapons;
    public List<Items> shields;

    //임시 돈
    public int money;

    public static ItemManager _Instance;
    public static ItemManager getInstance
    {
        get
        {
            if(_Instance == null)
            {
                GameObject temp = new GameObject("ItemManager");
                _Instance = temp.AddComponent<ItemManager>();
            }

            return _Instance;
        }
    }

    private void Awake()
    {
        if(!_Instance)
        {
            _Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        //money = 10000000;
        SetShopItems();
    }
    //아이템들 셋업하기
    public void SetShopItems()
    {
        //위에서 지정한 폴더와 파일 이름을 하나로 합쳐서
        string path = directoryName + fileNames;
        //정보 읽어올 준비
        TextAsset text = (TextAsset)Resources.Load(path);
        XmlDocument document = new XmlDocument();
        document.LoadXml(text.text);

        XmlElement temp = document["Items"];

        XmlNode dataNode = temp.FirstChild;
        XmlElement data;
        //정보의 수만큼 돌려서 
        for (int i = 0; i < temp.ChildNodes.Count; ++i, dataNode = dataNode.NextSibling)
        {
            data = (XmlElement)dataNode;
            Items newItem = new Items();  //임의 아이템하나 만들고
            // 속성에 맞게 데이터 안착
            int typeCount = System.Convert.ToInt16(data.GetAttribute("type"));
            
            switch (typeCount)
            {
                case 0:
                    newItem.type = Items.ItemType.Item_HeadGear;
                    break;
                case 1:
                    newItem.type = Items.ItemType.Item_Weapon;
                    break;
                case 2:
                    newItem.type = Items.ItemType.Item_Shield;
                    break;
            }

            newItem.itemName = data.GetAttribute("name");
            //string spritePath = "Sprite/" + newItem.itemName;  //이부분은 아이콘 스프라이트 정립 되면 주석을 풀어 사용할 예정
            //newItem.icon = (Sprite)Resources.Load(spritePath); //해당아이콘 집어넣기
            newItem.value = System.Convert.ToSingle(data.GetAttribute("value"));
            newItem.buyPrice = System.Convert.ToInt16(data.GetAttribute("price"));
            newItem.upgradePriceRatio = System.Convert.ToSingle(data.GetAttribute("upgradePriceRatio"));
            
            //임의 아이템의 타입에 따라 정보 분류 
            if (newItem.type == Items.ItemType.Item_HeadGear)       //투구이면
            {
                headgears.Add(newItem); //투구데이터들 안착
                headgears[headgears.Count - 1].index = headgears.Count - 1; // 투구데이터의 인덱스 부여
            }
            else if (newItem.type == Items.ItemType.Item_Weapon)    //무기이면
            {
                weapons.Add(newItem);   //마찬가지로 무기 테이터를 안착
                weapons[weapons.Count - 1].index = weapons.Count - 1;   //무기데이터의 인덱스 부여
            }
            else if (newItem.type == Items.ItemType.Item_Shield)    //방패이면
            {
                shields.Add(newItem);   // 방패 데이터 안착
                shields[shields.Count - 1].index = shields.Count - 1;   //방패데이터의 인덱스 부여
            }
        }

        CreateToItems();
    }
    //위에 정보들을 바탕으로 아이콘들을 생성하는 함수들
    void CreateToItems()
    {
        for(int i =0; i < headgears.Count; ++i)
        {
            GameObject newGameObject = Instantiate(itemSample, headgearsPosition);  //게임오브젝트 생성
            newGameObject.transform.position = new Vector3(960.0f + (i * 300.0f), 390.0f, 0);   //위치지정
            Items tempInfo = newGameObject.GetComponent<Items>();   //생성 오브젝트의 빈 아이템을 가지고 와서
            tempInfo.SetData(headgears[i]); //정보를 대입
            headgearObjects.Add(newGameObject); //생성한 오브젝트 제어를 위해 제어할 오브젝트 리스트에 넣어 관리.
        }       //이하 아래 함수 동일.
        
        for(int i =0; i < weapons.Count; ++i)
        {
            GameObject newGameObject = Instantiate(itemSample, weaponsPosition);
            newGameObject.transform.position = new Vector3(960.0f + (i * 300.0f), 440.0f, 0);
            Items tempInfo = newGameObject.GetComponent<Items>();
            tempInfo.SetData(weapons[i]);
            weaponObjects.Add(newGameObject);
        }

        for(int i =0; i < shields.Count; ++i)
        {
            GameObject newGameObject = Instantiate(itemSample, shieldsPosition);
            newGameObject.transform.position = new Vector3(960.0f + (i * 300.0f), 440.0f, 0);
            Items tempInfo = newGameObject.GetComponent<Items>();
            tempInfo.SetData(shields[i]);
            shieldObjects.Add(newGameObject);
        }
    }
    //현제 샾아이콘 인댁스에 따라 아이콘들을 나타낼것인가 감출 것인가를 제어하는 함수.
    public void CurrentItemPopups(int num)
    {
        switch(num)
        {
            case 0:
                for(int i = 0; i < headgearObjects.Count; ++i)
                {
                    headgearObjects[i].gameObject.SetActive(true);
                }
                for (int j = 0; j < weaponObjects.Count; ++j)
                {
                    weaponObjects[j].gameObject.SetActive(false);
                }
                for(int k = 0; k < shieldObjects.Count; ++k)
                {
                    shieldObjects[k].gameObject.SetActive(false);
                }
                break;
            case 1:
                for (int i = 0; i < headgearObjects.Count; ++i)
                {
                    headgearObjects[i].gameObject.SetActive(false);
                }
                for (int j = 0; j < weaponObjects.Count; ++j)
                {
                    weaponObjects[j].gameObject.SetActive(true);
                }
                for (int k = 0; k < shieldObjects.Count; ++k)
                {
                    shieldObjects[k].gameObject.SetActive(false);
                }
                break;
            case 2:
                for (int i = 0; i < headgearObjects.Count; ++i)
                {
                    headgearObjects[i].gameObject.SetActive(false);
                }
                for (int j = 0; j < weaponObjects.Count; ++j)
                {
                    weaponObjects[j].gameObject.SetActive(false);
                }
                for (int k = 0; k < shieldObjects.Count; ++k)
                {
                    shieldObjects[k].gameObject.SetActive(true);
                }
                break;
            default:
                for (int i = 0; i < headgearObjects.Count; ++i)
                {
                    headgearObjects[i].gameObject.SetActive(false);
                }
                for (int j = 0; j < weaponObjects.Count; ++j)
                {
                    weaponObjects[j].gameObject.SetActive(false);
                }
                for (int k = 0; k < shieldObjects.Count; ++k)
                {
                    shieldObjects[k].gameObject.SetActive(false);
                }
                break;
        }
    }
    //아이템 아이콘들이 움직이면서 틀어지는것을 방지하고자 만든 소팅(정렬)함수.
    public void SortItems(int index)
    {
        if(currentShopIndex == 0)
        {
            Vector2 basePos = new Vector2(960.0f, 390.0f);
            for (int i = 0; i < headgearObjects.Count; ++i)
            {
                headgearObjects[i].transform.position = new Vector2(basePos.x - ((index - i) * 300.0f), basePos.y);
            }
        }
        else if (currentShopIndex == 1)
        {
            Vector2 basePos = new Vector2(960.0f, 390.0f);
            for (int i = 0; i < weaponObjects.Count; ++i)
            {
                weaponObjects[i].transform.position = new Vector2(basePos.x - ((index - i) * 300.0f), basePos.y);
            }
        }
        else if (currentShopIndex == 2)
        {
            Vector2 basePos = new Vector2(960.0f, 390.0f);
            for (int i = 0; i < shieldObjects.Count; ++i)
            {
                shieldObjects[i].transform.position = new Vector2(basePos.x - ((index - i) * 300.0f), basePos.y);
            }
        }
    }
    //현재 각 탭의 현재 아이탬 인댁스에 맞게끔 움직이게 하는 함수들.
    public void MoveToHeadgearObjects(Vector2 goal, float speed)
    {
        if (headgearObjects[currentHeadgearIndex].transform.position.x < goal.x)
        {
            if (goal.x - headgearObjects[currentHeadgearIndex].transform.position.x <= 0.02)
            {
                headgearObjects[currentHeadgearIndex].transform.position = 
                    new Vector2(goal.x, headgearObjects[currentHeadgearIndex].transform.position.y);
            }
            else
            {
                for (int i = 0; i < headgearObjects.Count; ++i)
                {
                    headgearObjects[i].transform.Translate(speed, 0, 0);
                }
            }
        }
        else if (headgearObjects[currentHeadgearIndex].transform.position.x > goal.x)
        {
            if (headgearObjects[currentHeadgearIndex].transform.position.x - goal.x <= 0.02)
            {
                headgearObjects[currentHeadgearIndex].transform.position =
                    new Vector2(goal.x, headgearObjects[currentHeadgearIndex].transform.position.y);
            }
            else
            {
                for (int i = 0; i < headgearObjects.Count; ++i)
                {
                    headgearObjects[i].transform.Translate(-speed, 0, 0);
                }
            }
        }
    }
    public void MoveToWeaponObjects(Vector2 goal, float speed)
    {
        if (weaponObjects[currentWeaponIndex].transform.position.x < goal.x)
        {
            if (goal.x - weaponObjects[currentWeaponIndex].transform.position.x <= 0.02)
                weaponObjects[currentWeaponIndex].transform.position = 
                    new Vector2(goal.x, weaponObjects[currentWeaponIndex].transform.position.y);
            else
            {
                for (int i = 0; i < weaponObjects.Count; ++i)
                {
                    weaponObjects[i].transform.Translate(speed, 0, 0);
                }
            }
        }
        else if (weaponObjects[currentWeaponIndex].transform.position.x > goal.x)
        {
            if (weaponObjects[currentWeaponIndex].transform.position.x - goal.x <= 0.02)
                weaponObjects[currentWeaponIndex].transform.position = 
                    new Vector2(goal.x, weaponObjects[currentWeaponIndex].transform.position.y);
            else
            {
                for (int i = 0; i < weaponObjects.Count; ++i)
                {
                    weaponObjects[i].transform.Translate(-speed, 0, 0);
                }
            }
        }
    }
    public void MoveToShieldObjects(Vector2 goal, float speed)
    {
        if (shieldObjects[currentShieldIndex].transform.position.x < goal.x)
        {
            if (goal.x - shieldObjects[currentShieldIndex].transform.position.x <= 0.02)
                shieldObjects[currentShieldIndex].transform.position = 
                    new Vector2(goal.x, shieldObjects[currentShieldIndex].transform.position.y);
            else
            {
                for (int i = 0; i < shieldObjects.Count; ++i)
                {
                    shieldObjects[i].transform.Translate(speed, 0, 0);
                }
            }
        }
        else if (shieldObjects[currentShieldIndex].transform.position.x > goal.x)
        {
            if (shieldObjects[currentShieldIndex].transform.position.x - goal.x <= 0.02)
                shieldObjects[currentShieldIndex].transform.position =
                    new Vector2(goal.x, shieldObjects[currentShieldIndex].transform.position.y);
            else
            {
                for (int i = 0; i < shieldObjects.Count; ++i)
                {
                    shieldObjects[i].transform.Translate(-speed, 0, 0);
                }
            }
        }
    }
    //현재 각 탭의 아이템 인댁스에 맞게 아이템 아이콘들의 사이즈 조절하는 함수들
    public void CurrentHeadgearItems(float speed)
    {
        for(int i =0; i < headgearObjects.Count; ++i)
        {
            if(i == currentHeadgearIndex)
            {
                if (headgearObjects[i].transform.localScale.x < 1.5f)
                {
                    Vector2 scale = headgearObjects[i].transform.localScale;
                    scale.x += speed;
                    scale.y += speed;
                    headgearObjects[i].transform.localScale = scale;
                }
                else headgearObjects[i].transform.localScale = new Vector2(1.5f, 1.5f);
            }
            else
            {
                if (headgearObjects[i].transform.localScale.x > 1.0f)
                {
                    Vector2 scale = headgearObjects[i].transform.localScale;
                    scale.x -= speed;
                    scale.y -= speed;
                    headgearObjects[i].transform.localScale = scale;
                }
                else headgearObjects[i].transform.localScale = new Vector2(1.0f, 1.0f);
            }
        }
    }
    public void CurrentWeaponItems(float speed)
    {
        for (int i = 0; i < weaponObjects.Count; ++i)
        {
            if (i == currentWeaponIndex)
            {
                if (weaponObjects[i].transform.localScale.x < 1.5f)
                {
                    Vector2 scale = weaponObjects[i].transform.localScale;
                    scale.x += speed;
                    scale.y += speed;
                    weaponObjects[i].transform.localScale = scale;
                }
                else weaponObjects[i].transform.localScale = new Vector2(1.5f, 1.5f);
            }
            else
            {
                if (weaponObjects[i].transform.localScale.x > 1.0f)
                {
                    Vector2 scale = weaponObjects[i].transform.localScale;
                    scale.x -= speed;
                    scale.y -= speed;
                    weaponObjects[i].transform.localScale = scale;
                }
                else weaponObjects[i].transform.localScale = new Vector2(1.0f, 1.0f);
            }
        }
    }
    public void CurrentShieldItems(float speed)
    {
        for (int i = 0; i < shieldObjects.Count; ++i)
        {
            if (i == currentShieldIndex)
            {
                if (shieldObjects[i].transform.localScale.x < 1.5f)
                {
                    Vector2 scale = shieldObjects[i].transform.localScale;
                    scale.x += speed;
                    scale.y += speed;
                    shieldObjects[i].transform.localScale = scale;
                }
                else shieldObjects[i].transform.localScale = new Vector2(1.5f, 1.5f);
            }
            else
            {
                if (shieldObjects[i].transform.localScale.x > 1.0f)
                {
                    Vector2 scale = shieldObjects[i].transform.localScale;
                    scale.x -= speed;
                    scale.y -= speed;
                    shieldObjects[i].transform.localScale = scale;
                }
                else shieldObjects[i].transform.localScale = new Vector2(1.0f, 1.0f);
            }
        }
    }
    //샾아이콘 슬롯이 투구나 무기나 방패탭이 아닐경우 아이템이 보일 필요가 없기 떄문에 아이템 아이콘 전체를 끄는 함수
    public void AllItemActiveFalse()
    {
        for(int i=0; i <headgearObjects.Count; ++i) { headgearObjects[i].SetActive(false); }
        for(int i=0; i < weaponObjects.Count; ++i) { weaponObjects[i].SetActive(false); }
        for(int i=0; i<shieldObjects.Count; ++i) { shieldObjects[i].SetActive(false); }
    }

    
}
