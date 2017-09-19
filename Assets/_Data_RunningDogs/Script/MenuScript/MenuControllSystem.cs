using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControllSystem : MonoBehaviour
{
    //조작 컨트롤 하기 위한 타입
    public enum SceneType
    {
        Scene_Menu,
        Scene_Shop,
        Scene_Stage,
        Scene_Traning,
        Scene_TimeAtteck,
        Scene_UpdateNote
    }

    public SceneType type;
    public MenuControll menuCon;
    public ShopIconControll shopiconCon;
    public SelectToMonster monSelectCon;
    public Vector2 startPos;
    public Vector2 endPos;
    public float exception;
    public GameObject backButton;
    public GameObject setupPanel;

    void Start()
    {
        menuCon.index = 1;
        menuCon.SortMenus();
        if (EnemyManager.getInstance.icons.Count <= 0) monSelectCon.CreateToIcons();
    }

    private void Update()
    {
        ChackedPoses();


        if (menuCon.pop) backButton.SetActive(false);
        else backButton.SetActive(true);

        if(type == SceneType.Scene_Menu)
        {
            if (menuCon.moveToCenter) StartCoroutine(menuCon.MoveToCenter());
            if (menuCon.change) StartCoroutine(menuCon.ChangeToBackImageSize());
            if (menuCon.moveToMenus)
            {
                if (menuCon.index > 0) StartCoroutine(menuCon.MoveToMenus());
                StartCoroutine(menuCon.MenuIconSizeControll());
            }
            if (menuCon.ready) IdentifyTypes();
        }
        else if(type == SceneType.Scene_Shop)
        {
            if(!menuCon.pop)
            {
                switch(ItemManager.getInstance.currentShieldIndex)
                {
                    case 0:
                        if (ItemManager.getInstance.headgearObjects
                            [ItemManager.getInstance.currentHeadgearIndex].transform.position.x != 960.0f) shopiconCon.itemMove = true;
                        break;
                    case 1:
                        if (ItemManager.getInstance.weaponObjects
                            [ItemManager.getInstance.currentWeaponIndex].transform.position.x != 960.0f) shopiconCon.itemMove = true;
                        break;
                    case 2:
                        if (ItemManager.getInstance.shieldObjects
                            [ItemManager.getInstance.currentShieldIndex].transform.position.x != 960.0f) shopiconCon.itemMove = true;
                        break;
                }
            }
            if (shopiconCon.move)
            {
                StartCoroutine(shopiconCon.MoveToShopIcon());
                StartCoroutine(shopiconCon.CurrentIconScale());
            }
            else shopiconCon.SortIcons();

            if (shopiconCon.itemMove)
            {
                StartCoroutine(shopiconCon.MoveToShopItem());
                StartCoroutine(shopiconCon.CurrentItemScale());
            }
            else
            {
                switch(ItemManager.getInstance.currentShopIndex)
                {
                    case 0:
                        ItemManager.getInstance.SortItems(ItemManager.getInstance.currentHeadgearIndex);
                        break;
                    case 1:
                        ItemManager.getInstance.SortItems(ItemManager.getInstance.currentWeaponIndex);
                        break;
                    case 2:
                        ItemManager.getInstance.SortItems(ItemManager.getInstance.currentShieldIndex);
                        break;
                }
            }
        }
        else if(type == SceneType.Scene_Traning || type == SceneType.Scene_TimeAtteck)
        {
            if(monSelectCon.move)
            {
                StartCoroutine(monSelectCon.MoveControll());
                StartCoroutine(monSelectCon.ScaleControll());
            }
            if (EnemyManager.getInstance.index == EnemyManager.getInstance.icons.Count - 1)
                monSelectCon.rekoningToRandomIndex = true;
            if (monSelectCon.rekoningToRandomIndex) StartCoroutine(monSelectCon.RandomMonsterSelect());
        }
        else if(type == SceneType.Scene_Stage)
        {
            if (StageManager.getInstance.ready) SceneManager.LoadScene("InGameScene");
            else StageManager.getInstance.SortIcons();
        }
        
    }

    void ChackedPoses()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (startPos != Vector2.zero) startPos = Vector2.zero;
            if (endPos != Vector2.zero) endPos = Vector2.zero;

            startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            ResultToPoses();
        }

        //if(Input.touchCount > 0)
        //{
        //    if(Input.GetTouch(0).phase == TouchPhase.Began)
        //    {
        //        if (startPos != Vector2.zero) startPos = Vector2.zero;
        //        if (endPos != Vector2.zero) endPos = Vector2.zero;
        //
        //        startPos = Input.mousePosition;
        //    }
        //    else if(Input.GetTouch(0).phase == TouchPhase.Ended)
        //    {
        //        endPos = Input.mousePosition;
        //        ResultToPoses();
        //    }
        //}
    }

    void ResultToPoses()
    {
        if(type == SceneType.Scene_Menu)
        {
            if(startPos.x - endPos.x > exception)
            {
                if (menuCon.index < menuCon.menus.Length - 2)
                {
                    menuCon.moveToMenus = true;
                    menuCon.left = false;

                    menuCon.index++;
                }
            }
            else if(endPos.x - startPos.x > exception)
            {
                if (menuCon.index > 0)
                {
                    menuCon.moveToMenus = true;
                    menuCon.left = true;

                    menuCon.index--;
                }
            }
            menuCon.ReKoningToMoveSpeed();
        }
        else if(type == SceneType.Scene_Shop)
        {
            if(!shopiconCon.move)
            { 
                if(startPos.y - endPos.y > exception)
                {
                    if(ItemManager.getInstance.currentShopIndex >= 0)ItemManager.getInstance.currentShopIndex--;
                    if (ItemManager.getInstance.currentShopIndex < 0)
                        ItemManager.getInstance.currentShopIndex = shopiconCon.icons.Length - 1;
                    shopiconCon.move = true;
                    shopiconCon.up = true;
                }
                else if(endPos.y - startPos.y > exception)
                {
                    if (ItemManager.getInstance.currentShopIndex <= shopiconCon.icons.Length - 1)
                        ItemManager.getInstance.currentShopIndex++;
                    if (ItemManager.getInstance.currentShopIndex > shopiconCon.icons.Length - 1)
                        ItemManager.getInstance.currentShopIndex = 0;
                    shopiconCon.move = true;
                    shopiconCon.up = false;
                }
                shopiconCon.RekoningToSpeed();
            }
            else shopiconCon.itemMove = false;

            if (startPos.x - endPos.x > exception)
            {
                switch(ItemManager.getInstance.currentShopIndex)
                {
                    case 0:
                        if (ItemManager.getInstance.currentHeadgearIndex < ItemManager.getInstance.headgearObjects.Count - 1)
                            ItemManager.getInstance.currentHeadgearIndex++;
                        shopiconCon.itemMove = true;
                        shopiconCon.RekoningToSpeed();
                        break;
                    case 1:
                        
                        if (ItemManager.getInstance.currentWeaponIndex < ItemManager.getInstance.weaponObjects.Count - 1)
                            ItemManager.getInstance.currentWeaponIndex++;
                        shopiconCon.itemMove = true;
                        shopiconCon.RekoningToSpeed();
                        break;
                    case 2:
                        if (ItemManager.getInstance.currentShieldIndex < ItemManager.getInstance.shieldObjects.Count - 1)
                                ItemManager.getInstance.currentShieldIndex++;
                        shopiconCon.itemMove = true;
                        shopiconCon.RekoningToSpeed();
                        break;
                }
            }
            else if(endPos.x - startPos.x > exception)
            {
                switch (ItemManager.getInstance.currentShopIndex)
                {
                    case 0:
                        if (ItemManager.getInstance.currentHeadgearIndex > 0) ItemManager.getInstance.currentHeadgearIndex--;
                        shopiconCon.itemMove = true;
                        shopiconCon.RekoningToSpeed();
                        break;
                    case 1:
                        if (ItemManager.getInstance.currentWeaponIndex > 0) ItemManager.getInstance.currentWeaponIndex--;
                        shopiconCon.itemMove = true;
                        shopiconCon.RekoningToSpeed();
                        break;
                    case 2:
                        if (ItemManager.getInstance.currentShieldIndex > 0) ItemManager.getInstance.currentShieldIndex--;
                        shopiconCon.itemMove = true;
                        shopiconCon.RekoningToSpeed();
                        break;
                }
            }
        }
        else if(type == SceneType.Scene_Traning || type == SceneType.Scene_TimeAtteck)
        {
            if (!monSelectCon.move)
            { 
                if(startPos.x - endPos.x > exception)
                {
                    if (EnemyManager.getInstance.index < EnemyManager.getInstance.icons.Count -1)
                        EnemyManager.getInstance.index++;
                    monSelectCon.move = true;
                }
                else if(endPos.x - startPos.x > exception)
                {
                    if (EnemyManager.getInstance.index > 0) EnemyManager.getInstance.index--;
                    monSelectCon.move = true;
                }

                monSelectCon.RekoningMoveSpeed();
            }
        }

    }

    public void ClickToShopButton()
    {
        menuCon.index = 0;
    }
    public void ClickToStageButton()
    {
        menuCon.index = 1;
    }
    public void ClickToTraningButton()
    {
        menuCon.index = 2;
    }
    public void ClickToTimeAtteckButton()
    {
        menuCon.index = 3;
    }
    public void ClickToUpdatenoteButton()
    {
        menuCon.index = 4;
    }

    public void ClickToBackButton()
    {
        menuCon.index = 1;

        if (type == SceneType.Scene_Shop) shopiconCon.CloseGameObject();
        else if (type == SceneType.Scene_TimeAtteck || type == SceneType.Scene_Traning)
            monSelectCon.CloseToSelection();
        else if (type == SceneType.Scene_Stage)
            StageManager.getInstance.CloseControllPanel();

        for (int i = 0; i < menuCon.menus.Length; ++i)
        {
            menuCon.menus[i].gameObject.SetActive(true);
        }
        menuCon.pop = true;
        menuCon.SortMenus();
        menuCon.ready = false;
        type = SceneType.Scene_Menu;
        shopiconCon.shopPanel.SetActive(false);
    }

    public void ClickToSetupButton()
    {
        setupPanel.SetActive(true);
    }
    public void ClickToAtherArea()
    {
        setupPanel.SetActive(false);
    }
    void IdentifyTypes()
    {
        switch(menuCon.index)
        {
            case 0:
                type = SceneType.Scene_Shop;
                shopiconCon.WakeUpGameObject();
                break;
            case 1:
                type = SceneType.Scene_Stage;
                StageManager.getInstance.WakeUpControllPanel();
                break;
            case 2:
                type = SceneType.Scene_Traning;
                monSelectCon.WakeUpToSelection();
                break;
            case 3:
                type = SceneType.Scene_TimeAtteck;
                monSelectCon.WakeUpToSelection();
                break;
            case 4:
                type = SceneType.Scene_UpdateNote;
                break;
        }
    }

    public void ChangeToTraningScene()  { SceneManager.LoadScene("TraningScene");  }
    public void ChangeToStageScene()    { SceneManager.LoadScene("InGameScene"); }
} 
