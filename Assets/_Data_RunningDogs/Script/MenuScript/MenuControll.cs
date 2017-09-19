using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControll : MonoBehaviour
{
    public Image[] menus;       //매뉴이미지 배열
    public Image[] icons;       //매뉴의 아이콘 이미지 배열
    public Image back;          //백그라운드 이미지(매뉴 이미지가 알파일 경우를 대비하여)
    public Vector2 center;      //중심점 백터 
    public int index;           //매뉴 인댁스
    public float moveSpeed;     //움직이는 스피드
    public float scaleSpeed;    //아이콘변경 스피드
    public float changeTime;    //변경 시간
    public float widthLimite;   
    public float currentWidth;
    public bool moveToCenter;   
    public bool moveToMenus;
    public bool left;
    public bool change;
    public bool pop;
    public bool ready;
    public Camera cam;

    //움직일 스피드 계산 함수
	public void ReKoningToMoveSpeed()
    {
        if(menus[index].transform.position.x > center.x)    //센터보다 크면
            moveSpeed = (menus[index].transform.position.x - center.x) / (changeTime * 60);
        else if (menus[index].transform.position.x < center.x)  //센터보다 작으면
            moveSpeed = (center.x - menus[index].transform.position.x) / (changeTime * 60);
        
    }
	//인댁스 변경에 따른 배경색 변경 함수
    public void ChangeBackColor()
    {
        for(int i = 0; i<menus.Length; ++i)
        {
            if (i == index)
            {
                back.color = menus[index].color;
            }
            else menus[i].gameObject.SetActive(false);
        }
        moveToCenter = true;

    }
    //아이콘 클릭함수
    public void ClickToIcon()
    {
        if(index > 0 && index < icons.Length -1)moveToMenus = true;
        if (menus[index].transform.position.x < center.x) left = true;
        else left = false;
    }
    //가운데로 움직이게 하는 코루틴
    public IEnumerator MoveToCenter()
    {
        if (menus[index].transform.position.x > center.x) //현매뉴가 센터보다 크면
        {
            //현매뉴 이미지가 센터에 도착하면
            if (menus[index].transform.position.x - moveSpeed <= center.x - 0.02f)  
            {
                menus[index].transform.position = center;
                menus[index].gameObject.SetActive(false);
                back.gameObject.SetActive(true);
                moveToCenter = false;
                change = true;
            }
            //그게 아니면 이동
            else menus[index].transform.Translate(-moveSpeed, 0, 0);
        }
        else if (menus[index].transform.position.x < center.x)  //현매뉴가 센터보다 작으면
        {
            //현매뉴 이미지가 센터에 도착하면
            if (menus[index].transform.position.x + moveSpeed >= center.x + 0.02f)
            {
                menus[index].transform.position = center;
                menus[index].gameObject.SetActive(false);
                back.gameObject.SetActive(true);
                moveToCenter = false;
                change = true;
            }
            //그게 아니면 이동
            else menus[index].transform.Translate(moveSpeed, 0, 0);
        }
        else //현매뉴 센터이면
        {
            menus[index].gameObject.SetActive(false);
            back.gameObject.SetActive(true);
            moveToCenter = false;
            change = true;
        }
        yield return null;
    }
    //센터로 이동하면서 가운데 아이콘 이미지 사이즈 변경
    public IEnumerator ChangeToBackImageSize()
    {
        if(back.rectTransform.rect.width < widthLimite) //리미트보다 작으면
        {

            if(currentWidth <= 3.0f) currentWidth += Time.deltaTime * 6;
            else
            {
                change = false;
                pop = false;
                currentWidth = 1;
                cam.backgroundColor = back.color;
                back.gameObject.SetActive(false);
                ready = true;
            }

            back.transform.localScale = new Vector2(currentWidth, 1);
        }

        yield return null;
    }
    //아이콘들을 정렬한다.
    public void SortMenus()
    {
        for(int i = 0; i < menus.Length; ++i)
        {
            if (i == index)
            {
                menus[i].transform.position = center;
                icons[i].transform.localScale = new Vector2(1.5f, 1.5f);
            }
            else if (i > index)
            {
                menus[i].transform.position = new Vector2(center.x + ((i - index) * 640.0f), center.y);
                if (i != menus.Length - 1) icons[i].transform.localScale = new Vector2(1.0f, 1.0f);
            }
            else
            {
                menus[i].transform.position = new Vector2(center.x - ((index - i) * 640.0f), center.y);
                icons[i].transform.localScale = new Vector2(1.0f, 1.0f);
            }
        }
    }
    
    public IEnumerator MoveToMenus()
    {
        if(left)
        {
            if (menus[index].transform.position.x + moveSpeed >= center.x + 0.02f)
            {
                menus[index].transform.position = center;
                moveToMenus = false;
                left = false;
                SortMenus();
            }
            else
            {
                for (int i = 0; i < menus.Length; ++i)
                {
                    menus[i].transform.Translate(moveSpeed, 0, 0);
                }
            }
        }
        else
        {
            if (menus[index].transform.position.x - moveSpeed <= center.x - 0.02f)
            {
                menus[index].transform.position = center;
                moveToMenus = false;
                SortMenus();
            }
            else
            {
                for (int i = 0; i < menus.Length; ++i)
                {
                    menus[i].transform.Translate(-moveSpeed, 0, 0);
                }
            }
        }

        yield return null;
    }
    public IEnumerator MenuIconSizeControll()
    {
        for(int i =0; i < icons.Length; ++i)
        {
            if(i == index)
            {
                if (icons[i].transform.localScale.x <= 1.5f)
                {
                    Vector2 scale = icons[i].transform.localScale;
                    scale.x += scaleSpeed;
                    scale.y += scaleSpeed;
                    icons[i].transform.localScale = scale;
                }
                else
                {
                    icons[i].transform.localScale = new Vector2(1.5f, 1.5f);
                    moveToMenus = false;
                }
            }
            else
            {
                if (icons[i].transform.localScale.x > 1.0f)
                {
                    Vector2 scale = icons[i].transform.localScale;
                    scale.x -= scaleSpeed;
                    scale.y -= scaleSpeed;
                    icons[i].transform.localScale = scale;
                }
                else icons[i].transform.localScale = new Vector2(1.0f, 1.0f);
            }
        }

        yield return null;
    }
    
}
