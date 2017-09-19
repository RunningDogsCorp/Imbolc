using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class ShopIconControll : MonoBehaviour
{
    public Image[] icons;           //아이템 종류 아이콘
    public Vector2 center;          //중심점 백터
    public float speed;             //이동속도
    public float scaleSpeed;        //크기조절 속도
    public float moveTime;          //이동시간
    public bool move;               //움직이고 있는지에 따른 불값
    public bool up;                 //버티컬 방향 움직임 제어 불값
    public bool itemMove;           //아이콘 안쪽 아이템들이 움직이는지 제어하는 불값
    public bool pop;
    public Button buyButton;        //임의 상점 매뉴(캐쉬, 개발자에게 커피한잔) 부분에서 구매할 수 있도록 하는 사기 버튼.
    public GameObject shopPanel;    //샾 매뉴에 들어오고 나가고 할떄 패널을 제어할 오브젝트

    private void Start()
    {
        SortIcons();    //시작하면 소팅한번 돌리고
    }
    //아이콘을 클릭했을때 쓰는 그 아이콘이 중점이 아니라면 중점으로 움직이게 하는 버튼용 함수
    public void ClickToIcon()
    {
        move = true;    //움직이고  
        // 아이콘의 y축값을 센터와 비교해서 그보다 크면 위에서 아래로 아니면 아래서 위로 이동 제어값 
        if (icons[ItemManager.getInstance.currentShopIndex].transform.position.y > center.y) up = true;
        else up = false;
    }
    //움직이는 스피트 계산 함수
    public void RekoningToSpeed()
    {
        if(!itemMove)  //아이콘이 움직일때 속도 계산
        {
            if (icons[ItemManager.getInstance.currentShopIndex].transform.position.y > center.y)
            {
                speed = (icons[ItemManager.getInstance.currentShopIndex].transform.position.y - center.y) / (moveTime * 60.0f);
                scaleSpeed = 100.0f / (moveTime * 60.0f);
            }
            else if (icons[ItemManager.getInstance.currentShopIndex].transform.position.y < center.y)
            {
                speed = (center.y - icons[ItemManager.getInstance.currentShopIndex].transform.position.y) / (moveTime * 60.0f);
                scaleSpeed = 100.0f / (moveTime * 60.0f);
            }
        }
        else      //아이템이 움직일떄 속도 계산
        {
            switch (ItemManager.getInstance.currentShopIndex)
            {
                case 0:
                    if(ItemManager.getInstance.headgearObjects[ItemManager.getInstance.currentHeadgearIndex].transform.position.x < center.x)
                    {
                        speed = (center.x - ItemManager.getInstance.headgearObjects[ItemManager.getInstance.currentHeadgearIndex].
                        transform.position.x) / (moveTime * 60.0f);
                    }
                    else if(ItemManager.getInstance.headgearObjects[ItemManager.getInstance.currentHeadgearIndex].transform.position.x > center.x)
                    {
                        speed = (ItemManager.getInstance.headgearObjects[ItemManager.getInstance.currentHeadgearIndex].
                        transform.position.x - center.x) / (moveTime * 60.0f);
                    }
                    break;
                case 1:
                    if (ItemManager.getInstance.weaponObjects[ItemManager.getInstance.currentWeaponIndex].transform.position.x < center.x)
                    {
                        speed = (center.x - ItemManager.getInstance.weaponObjects[ItemManager.getInstance.currentWeaponIndex].
                        transform.position.x) / (moveTime * 60.0f);
                    }
                    else if (ItemManager.getInstance.weaponObjects[ItemManager.getInstance.currentWeaponIndex].transform.position.x > center.x)
                    {
                        speed = (ItemManager.getInstance.weaponObjects[ItemManager.getInstance.currentWeaponIndex].
                        transform.position.x - center.x) / (moveTime * 60.0f);
                    }
                    break;
                case 2:
                    if (ItemManager.getInstance.shieldObjects[ItemManager.getInstance.currentShieldIndex].transform.position.x < center.x)
                    {
                        speed = (center.x - ItemManager.getInstance.shieldObjects[ItemManager.getInstance.currentShieldIndex].
                        transform.position.x) / (moveTime * 60.0f);
                    }
                    else if (ItemManager.getInstance.shieldObjects[ItemManager.getInstance.currentShieldIndex].transform.position.x > center.x)
                    {
                        speed = (ItemManager.getInstance.shieldObjects[ItemManager.getInstance.currentShieldIndex].
                        transform.position.x - center.x) / (moveTime * 60.0f);
                    }
                    break;
            }
            scaleSpeed = 0.5f / (moveTime * 60.0f);
        }
    }
    //아이콘 움직일때 연출 제어 코루틴
    public IEnumerator MoveToShopIcon()
    {
        if(move)    //움직이면
        {
            if(up)  //위로 움직이면
            {
                if(icons[ItemManager.getInstance.currentShopIndex].transform.position.y - center.y <= 0.02f) //센터에 근접하거나 넘어가면
                {
                    icons[ItemManager.getInstance.currentShopIndex].transform.position = center; //센터에 위치를 맞추고
                    move = false;   //움직임을 멈추고
                    up = false;     //위 아래인지 판단여부를 펄스로 초기화
                    SortIcons();    //아이콘 위치가 틀어질 수 있으니 다시한번 소팅
                    ItemManager.getInstance.CurrentItemPopups(ItemManager.getInstance.currentShopIndex);

                    if (ItemManager.getInstance.currentShopIndex > 2)   //임의 매뉴인댁스부분이면
                    {
                        buyButton.gameObject.SetActive(true);           //버튼 활성화
                    }
                    else buyButton.gameObject.SetActive(false);         //그게 아니면 비활성화
                }
                else     //아직 센터에 근접하지 않았다면
                {
                    ItemManager.getInstance.AllItemActiveFalse();   //아이템 아이콘이 나타나면 안되니깐 꺼주고
                    for (int i = 0; i < icons.Length; ++i)          //아이콘들을 계산한 속도에 맞춰 이동
                    {
                        icons[i].transform.Translate(0, -speed, 0);
                    }
                    buyButton.gameObject.SetActive(false);          //아이콘이 움직이면 무조건 버튼 비활성화
                }
            }
            else
            {
                if(center.y - icons[ItemManager.getInstance.currentShopIndex].transform.position.y <= 0.02f)
                {
                    icons[ItemManager.getInstance.currentShopIndex].transform.position = center;
                    move = false;
                    SortIcons();
                    ItemManager.getInstance.CurrentItemPopups(ItemManager.getInstance.currentShopIndex);

                    if (ItemManager.getInstance.currentShopIndex > 2)   //임의 매뉴인댁스부분이면
                    {
                        buyButton.gameObject.SetActive(true);           //버튼 활성화
                    }
                    else buyButton.gameObject.SetActive(false);         //그게 아니면 비활성화
                }
                else
                {
                    ItemManager.getInstance.AllItemActiveFalse();
                    for (int i =0; i < icons.Length; ++i)
                    {
                        icons[i].transform.Translate(0, speed, 0);
                    }
                    buyButton.gameObject.SetActive(false);              //움직임이 있을떄는 무조건 버튼 비활성화
                }
            }
        }

        yield return null;
    }

    //아이템 아이콘들의 이동에 대한 코루틴
    public IEnumerator MoveToShopItem()
    {
        switch (ItemManager.getInstance.currentShopIndex)   //지금현제의 어떤 장비부분을 보고 있는지에 따라
        {
            case 0:     //투구인가?
                ItemManager.getInstance.MoveToHeadgearObjects(center, speed);//투구면 센터까지 계산된 속도로 움직이게 한다.
                break;
            case 1:     //무기인가?
                ItemManager.getInstance.MoveToWeaponObjects(center, speed); //무기면 센터까지계산된 속도로 움직이게 한다
                break;
            case 2:     //방패인가
                ItemManager.getInstance.MoveToShieldObjects(center, speed); //방패면 센터까지 계산된 속도로 움직이게 한다.
                break;
        }

        yield return null;
    }

    //현제 아이콘이 눈에 띌 수 있게 스케일 조절하는 코루틴 함수
    public IEnumerator CurrentIconScale()
    {
        for(int i =0; i < icons.Length; ++i)
        {
            if(i == ItemManager.getInstance.currentShopIndex) //아이콘의 인덱스가 맞다면
            {
                if (icons[i].rectTransform.rect.width < 300.0f) //아이콘의 사각형이 최종크기(일단 300.0f로 잡아놓음)보다 작으면
                {
                    float width = icons[i].rectTransform.rect.width;    //넓이와
                    float height = icons[i].rectTransform.rect.height;  //높이를 받아와서

                    width += scaleSpeed;    //계산된 속도를 더해줘서 키운다
                    height += scaleSpeed;

                    icons[i].rectTransform.sizeDelta = new Vector2(width, height);  // 키운값을 아이콘의 사각형에 대입
                }
                else icons[i].rectTransform.sizeDelta = new Vector2(300.0f, 300.0f);    // 아이콘의 최종크기와 갇거나 크면 크기 고정.
            }
            else //아이콘의 인덱스가 안맞는 것들은
            {
                if (icons[i].rectTransform.rect.width > 200.0f) //아이콘 사각형의 최소크기(일단 200.0f로 잡아놓음) 보다 크면
                {
                    float width = icons[i].rectTransform.rect.width;    //넓이와
                    float height = icons[i].rectTransform.rect.height;  //높이를 받아와서

                    width -= scaleSpeed;    //위와 같이 계산된 속도로 깎아준다.
                    height -= scaleSpeed;

                    icons[i].rectTransform.sizeDelta = new Vector2(width, height); //그후 대입
                }
                else icons[i].rectTransform.sizeDelta = new Vector2(200.0f, 200.0f);    //아이콘의 최소크기와 같거나 작아지면 크기 고정.
            }
        }

        yield return null;
    }

    //아이템의 스케일조절 하는 코루틴 함수
    public IEnumerator CurrentItemScale()
    {
        switch(ItemManager.getInstance.currentShopIndex)    //현재 어떤 부분을 보고있는지에 따라
        {
            case 0: //투구면
                ItemManager.getInstance.CurrentHeadgearItems(scaleSpeed); //투구아이템들 움직이는 함수에 속도 대입
                break;
            case 1: //무기면
                ItemManager.getInstance.CurrentWeaponItems(scaleSpeed); //무기아이템들 움직이는 함수에 속도 대입
                break;
            case 2: //방패면
                ItemManager.getInstance.CurrentShieldItems(scaleSpeed); //방패아이템들움직이는 함수에 속도 대입
                break;
        }
        yield return null;
    }

    //아이템 정렬 함수
    public void SortIcons()
    {
        //스위치를 쓴 이유는 혹시나 있을 트러짐 현상이 일어나면 그정적인 값으로 바로 잡기 위해 스위치 사용
        // 0번 : 투구
        // 1번 : 무기
        // 2번 : 방패
        // 3번 : 캐쉬(향후 있을 것을 대비함.)
        // 4번 : 개발자에게 커피한잔 (향후 있을 것을 대비함.)
        switch (ItemManager.getInstance.currentShopIndex)
        {
            case 0: 
                icons[0].transform.position = center;
                icons[0].rectTransform.sizeDelta = new Vector2(300.0f, 300.0f);
                icons[1].transform.position = new Vector2(960.0f, 210.0f);
                icons[2].transform.position = new Vector2(960.0f, -120.0f);
                icons[3].transform.position = new Vector2(960.0f, 1200.0f);
                icons[4].transform.position = new Vector2(960.0f, 870.0f);
                break;
            case 1: 
                icons[1].transform.position = center;
                icons[1].rectTransform.sizeDelta = new Vector2(300.0f, 300.0f);
                icons[2].transform.position = new Vector2(960.0f, 210.0f);
                icons[3].transform.position = new Vector2(960.0f, -120.0f);
                icons[4].transform.position = new Vector2(960.0f, 1200.0f);
                icons[0].transform.position = new Vector2(960.0f, 870.0f);
                break;
            case 2:
                icons[2].transform.position = center;
                icons[2].rectTransform.sizeDelta = new Vector2(300.0f, 300.0f);
                icons[3].transform.position = new Vector2(960.0f, 210.0f);
                icons[4].transform.position = new Vector2(960.0f, -120.0f);
                icons[1].transform.position = new Vector2(960.0f, 870.0f);
                icons[0].transform.position = new Vector2(960.0f, 1200.0f);
                break;
            case 3:
                icons[3].transform.position = center;
                icons[3].rectTransform.sizeDelta = new Vector2(300.0f, 300.0f);
                icons[4].transform.position = new Vector2(960.0f, 210.0f);
                icons[0].transform.position = new Vector2(960.0f, -120.0f);
                icons[2].transform.position = new Vector2(960.0f, 870.0f);
                icons[1].transform.position = new Vector2(960.0f, 1200.0f);
                break;
            case 4:
                icons[4].transform.position = center;
                icons[4].rectTransform.sizeDelta = new Vector2(300.0f, 300.0f);
                icons[0].transform.position = new Vector2(960.0f, 210.0f);
                icons[1].transform.position = new Vector2(960.0f, -120.0f);
                icons[2].transform.position = new Vector2(960.0f, 1200.0f);
                icons[3].transform.position = new Vector2(960.0f, 870.0f);
                break;
        }
        //위에꺼는 위치였다면 아래꺼는 크기 정렬
        for (int i = 0; i < icons.Length; ++i)
        {
            //현재 인댁스와 같지 않은 아이콘은 200.0f(임의 고정값)으로 고정
            if (i != ItemManager.getInstance.currentShopIndex) icons[i].rectTransform.sizeDelta = new Vector2(200.0f, 200.0f);
            else continue;
        }
    }
    //샾 창이 켜지면 샾용 패널 활성화
    public void WakeUpGameObject()
    {
        shopPanel.SetActive(true);
        ItemManager.getInstance.AllItemActiveFalse();
    }
    //나가기 누르면 샾용 패널 비활성화
    public void CloseGameObject() { shopPanel.SetActive(false); }

    //아이템 사기
    public void BuyItems()
    {
        if (ItemManager.getInstance.currentShopIndex == 0)
        {
            //아이템사기 충분한 돈이 있다면
            if(ItemManager.getInstance.headgears[ItemManager.getInstance.currentHeadgearIndex].buyPrice <= ItemManager.getInstance.money)
            {
                //아이템의 값만큼 플레이어의 돈을 깎고 
                ItemManager.getInstance.money -= ItemManager.getInstance.headgears[ItemManager.getInstance.currentHeadgearIndex].buyPrice;
                //해당 아이템을 구입했다는 불값을 투르로 바꿔주고(중복 구매 방지용 불값, 필요에 따라 수정가능)
                ItemManager.getInstance.headgears[ItemManager.getInstance.currentHeadgearIndex].getItem = true;
                //인벤토리에 넣을 임시 아이템을 만들어서
                Items tempItem = ItemManager.getInstance.headgearObjects[ItemManager.getInstance.currentHeadgearIndex].GetComponent<Items>();
                tempItem.getItem = ItemManager.getInstance.headgears[ItemManager.getInstance.currentHeadgearIndex].getItem;
                PlayerInventory.getInven.BuyItemInShop(tempItem); //인벤토리에 안착 및 변경이 되었음으로 인벤토리 저장.
                //아래 있는 함수도 같은 원리로 작동.
            }
        }
        else if(ItemManager.getInstance.currentShopIndex == 1)
        {
            if (ItemManager.getInstance.weapons[ItemManager.getInstance.currentWeaponIndex].buyPrice <= ItemManager.getInstance.money)
            {
                ItemManager.getInstance.money -= ItemManager.getInstance.weapons[ItemManager.getInstance.currentWeaponIndex].buyPrice;
                ItemManager.getInstance.weapons[ItemManager.getInstance.currentWeaponIndex].getItem = true;
                Items tempItem = ItemManager.getInstance.weaponObjects[ItemManager.getInstance.currentWeaponIndex].GetComponent<Items>();
                tempItem.getItem = ItemManager.getInstance.weapons[ItemManager.getInstance.currentWeaponIndex].getItem;
                PlayerInventory.getInven.BuyItemInShop(tempItem);
            }
        }
        else if (ItemManager.getInstance.currentShopIndex == 2)
        {
            if (ItemManager.getInstance.shields[ItemManager.getInstance.currentShieldIndex].buyPrice <= ItemManager.getInstance.money)
            {
                ItemManager.getInstance.money -= ItemManager.getInstance.shields[ItemManager.getInstance.currentShieldIndex].buyPrice;
                ItemManager.getInstance.shields[ItemManager.getInstance.currentShieldIndex].getItem = true;
                Items tempItem = ItemManager.getInstance.shieldObjects[ItemManager.getInstance.currentShieldIndex].GetComponent<Items>();
                tempItem.getItem = ItemManager.getInstance.shields[ItemManager.getInstance.currentShieldIndex].getItem;
                PlayerInventory.getInven.BuyItemInShop(tempItem);
            }
        }
    }
}
