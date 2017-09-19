using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public enum ItemType
    {
        Item_HeadGear =0,
        Item_Weapon,
        Item_Shield
    }
        
    public ItemType type;              //아이템타입
    public Image icon;                 //아이콘 이미지
    public string itemName;            //이름
    public int id;                     //아이디
    public int index;                  //인덱스
    public int lv;                     //레벨
    public float value;                //적용수치
    public int buyPrice;               //살때가격
    public int upgradePrice;           //업그레이드가격
    public float upgradePriceRatio;    //업그래이드가격 상승비율
    public bool getItem;               //아이탬을 가지고 있는지 없는지
    public bool equip;                 //장비했는지 여부
    

    //매니저에 데이터를 저장하기 위한 함수. 샾에서 아이템들이 가지고 있는 정보를 넘기기 위해 사용.
    public void SetData(Items item)
    {
        itemName = item.itemName;
        type = item.type;
        id = item.id;
        icon = item.icon;
        index = item.index;
        lv = item.lv;
        value = item.value;
        buyPrice = item.buyPrice;
        upgradePriceRatio = item.upgradePriceRatio;
        upgradePrice = (int)((float)buyPrice * upgradePriceRatio);
        
    }
}
