using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class PlayerInventory : MonoBehaviour
{
    public List<Items> inventorySlots; //구매한 아이탬을 저장하는 배열
    //아이탬 장착 변수들
    Items headgearSlot; //인벤에서 쓸 고정변수
    Items weaponSlot;   //인벤에서 쓸 고정변수
    Items shieldSlot;   //인벤에서 쓸 고정변수
    public string dataDirectFolderName; //데이터 파일의 이름
    public bool move;   //인벤이 움직이는지 여부를 판단할 불값.
    public bool open;   //인벤 여닫음 제어용 불값.
    public float speed; //인벤창 왔다갔다 할 속도.

    static PlayerInventory inven;
    public static PlayerInventory getInven
    {
        get
        {
            if (inven == null)
            {
                GameObject newGameObject = new GameObject("Inventory");
                inven = newGameObject.GetComponent<PlayerInventory>();
            }
            return inven;
        }
    }


    public void SaveItems(string path) //아이템을 보유하고 있을떄 혹은 상점에서 변경사항이 있을떄쓸 함수
    {
        if(inventorySlots.Count > 0) //인벤토리가 비어있지 않으면
        {
            XmlDocument document = new XmlDocument();
            XmlElement temp = document.CreateElement("inventoryItems");
            document.AppendChild(temp);

            foreach(Items item in inventorySlots)
            {
                XmlElement block = document.CreateElement("Item");
                block.SetAttribute("itemType", item.type.ToString());
                block.SetAttribute("itemName", item.name);
                block.SetAttribute("lv", item.lv.ToString());
                block.SetAttribute("index", item.index.ToString());
                block.SetAttribute("buyPrice", item.buyPrice.ToString());
                block.SetAttribute("upgradePrice", item.upgradePrice.ToString());
                block.SetAttribute("upgradePriceRatio", item.upgradePriceRatio.ToString());
                block.SetAttribute("isGetItem", item.getItem.ToString());
                block.SetAttribute("isEquip", item.equip.ToString());

                temp.AppendChild(block);
            }

            document.Save(path);
        }
    }

    public void LoadItems(string path)
    {
        //리스트로 데이터가 관리되고 있기떄문에 중복이나 데이터가 꼬이는걸 방지 하기위해 
        //인벤토리 초기화
        if (inventorySlots.Count > 0) inventorySlots.Clear();

        string directoryPath = "Resources/xml/" + path;

        TextAsset text = (TextAsset)Resources.Load(directoryPath);
        XmlDocument document = new XmlDocument();
        document.LoadXml(text.text);

        XmlElement temp = document["Items"];

        XmlNode dataNode = temp.FirstChild;
        XmlElement data;

        //Xml파일 파싱해서 데이터 집어 넣기
        for (int i = 0; i < temp.ChildNodes.Count; ++i, dataNode = dataNode.NextSibling)
        {
            data = (XmlElement)dataNode;
            Items newItem = new Items();

            int typeCount = System.Convert.ToInt16(data.GetAttribute("itemType"));
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
            newItem.itemName = data.GetAttribute("itemName");
            newItem.lv = System.Convert.ToInt16(data.GetAttribute("lv"));
            newItem.index = System.Convert.ToInt16(data.GetAttribute("index"));
            newItem.value = System.Convert.ToInt16(data.GetAttribute("value"));
            newItem.buyPrice = System.Convert.ToInt16(data.GetAttribute("buyPrice"));
            newItem.upgradePrice = System.Convert.ToInt16(data.GetAttribute("upgradePrice"));
            newItem.upgradePriceRatio = System.Convert.ToSingle(data.GetAttribute("upgradePriceRatio"));
            string getItemString = data.GetAttribute("isGetItem");
            if (getItemString == "false") newItem.getItem = false;
            else newItem.getItem = true;
            string equipString = data.GetAttribute("isEquip");
            if (equipString == " false") newItem.equip = false;
            else newItem.equip = true;

            //함수 초기에 초기화 시켜놓았던 리스트에 넣어준다.
            inventorySlots.Add(newItem);
        }
    }

	void Start ()
    {
        LoadItems(dataDirectFolderName); //시작하면 아이템 목록을 읽어 온다.
	}

    public void ChangeEquipSystem(Items item)
    {
        if(item.type == Items.ItemType.Item_HeadGear)
        {
            headgearSlot = item;
        }
        else if(item.type == Items.ItemType.Item_Weapon)
        {
            weaponSlot = item;
        }
        else if(item.type == Items.ItemType.Item_Shield)
        {
            shieldSlot = item;
        }
    }

    //투구의 능력을 얻어갈 수 있는 겟터
    public float GetheadgearValue()
    {
        return headgearSlot.value;
    }
    //무기의 능력을 얻어갈 수 있는 겟터
    public float GetWeaponValue()
    {
        return weaponSlot.value;
    }
    //방패의 능력을 얻어갈 수 있는 겟터
    public float GetShieldValue()
    {
        return shieldSlot.value;
    }
    //아이템을 샀을때 데이터를 저장하는 함수.
    public void BuyItemInShop(Items item)
    {
        inventorySlots.Add(item);
        SaveItems(dataDirectFolderName);
    }

    //인벤토리 출연 연출 코루틴
    public IEnumerator MoveControllToInven()
    {
        if(move)    //움직임이 있을때
        {
            if (open)   //열려있으면
            {
                if (this.gameObject.transform.position.x < 2227.0f)     //지정범위까지 가지 못했다면
                {
                    this.gameObject.transform.Translate(speed, 0, 0);   //지정 속도 이동
                }
                else
                {
                    this.gameObject.transform.position = new Vector3(2227.0f,
                        this.gameObject.transform.position.y, this.gameObject.transform.position.z);    //도착했으면 위치정.
                }
            }
            else         //닫혀있으면
            {
                if (this.gameObject.transform.position.x > 1513.0f)     //지정범위까지 가지 못했다면
                {
                    this.gameObject.transform.Translate(-speed, 0, 0);  //지정 속도로 이동.
                }
                else
                {
                    this.gameObject.transform.position = new Vector3(1513.0f,
                        this.gameObject.transform.position.y, this.gameObject.transform.position.z);   //도착했다면 위치 고정.
                }
            }
        }

        yield return null;
    }
}
