using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScenarioIntro_Alpha : MonoBehaviour {

    //알파 샘플 시나리오.
    
    public PlayerActionScript player;
    public Npc_ActionScript[] enemies;
    public Npc_ActionScript[] friends;

    public Camera mainCam;
    public StageControlScript stage;

    public Text introQuote;

    bool act3_camControl;
    public bool complete;

    // Use this for initialization
    void Start()
    {
        StartMainGame();
        /*
        //시나리오 시작
        StartCoroutine(Setup());
        act3_camControl = false;

        for(int i=0; i < friends.Length; ++i)
        {
            friends[i].gameObject.SetActive(true);
        }
        */
    }

    IEnumerator Setup()
    {
        //액트 0 : 시나리오에 필요한 것들을 미리 세팅
        mainCam.transform.position = new Vector3(mainCam.transform.position.x,
            10.0f,
            mainCam.transform.position.z);
        
        yield return null;

        //세팅이 끝나면 액트 1부터 차례로 돌린다
        StartCoroutine(Act1());
    }

    IEnumerator Act1()
    {
        Debug.Log("Act 1 started.");

        introQuote.gameObject.SetActive(true);
        introQuote.color = new Color(introQuote.color.r, introQuote.color.g, introQuote.color.b, 0);
        float alpha = 0;

        while (introQuote.color.a < 1)
        {
            alpha += 0.05f;
            introQuote.color = new Color(introQuote.color.r, introQuote.color.g, introQuote.color.b, alpha);

            if (alpha >= 1)
            {
                yield return new WaitForSeconds(2);
                break;
            }
            yield return null;
        }

        while (introQuote.color.a > 0)
        {
            alpha -= 0.05f;
            introQuote.color = new Color(introQuote.color.r, introQuote.color.g, introQuote.color.b, alpha);

            if (alpha <= 0)
            {
                break;
            }
            yield return null;
        }

        StartCoroutine(Act2());
    }

    IEnumerator Act2()
    {
        Debug.Log("Act 2 started.");

        while (mainCam.transform.position.y > 1.04f)
        {
            mainCam.transform.Translate(0, -0.15f, 0);

            if (mainCam.transform.position.y <= 1.04f)
            {
                mainCam.transform.position = new Vector3(mainCam.transform.position.x, 1.04f, -10.0f);
                break;
            }

            yield return null;
        }

        StartCoroutine(Act3a());
    }

    IEnumerator Act3a()
    {
        act3_camControl = true;

        while (friends[0].transform.position.x > -15)
        {
            if (act3_camControl == true &&
                friends[0].transform.position.x < mainCam.transform.position.x)
            {
                mainCam.transform.position = new Vector3(
                friends[0].transform.position.x,
                mainCam.transform.position.y,
                mainCam.transform.position.z);
            }

            for (int i = 0; i < friends.Length; ++i)
            {
                //friends[i].transform.Translate(-0.1f, 0, 0);
                friends[i].MoveAction(Npc_BaseScript.ACTION.MOVE_BACK);
            }

            if (friends[0].transform.position.x < -3)
            {
                if (act3_camControl) StartCoroutine(Act3b());
            }

            if (friends[0].transform.position.x <= -15)
            {
                for (int i = 0; i < friends.Length; ++i)
                {
                    //디버그 코드. 리소스가 모이고 환경이 안정되면 SetActive(false)로 바꾼다.
                    friends[i].ReleaseAction();
                }

                break;
            }

            yield return null;
        }

        Debug.Log("act3a ended.");
    }

    IEnumerator Act3b()
    {
        act3_camControl = false;

        StartCoroutine(Act4());

        yield return null;

        Debug.Log("act3b ended.");
    }

    IEnumerator Act4()
    {
        while (enemies[0].transform.position.x > -2.0f)
        {
            for (int i = 0; i < enemies.Length; ++i)
            {
                enemies[i].MoveAction(Npc_BaseScript.ACTION.MOVE_FAST_FORWARD);
                enemies[i].transform.Translate(-0.03f, 0, 0);
            }

            if (enemies[0].transform.position.x <= -2.0f)
            {
                enemies[0].transform.position = new Vector2(
                    -2.0f,
                    enemies[0].transform.position.y);

                for (int i = 0; i < enemies.Length; ++i)
                {
                    enemies[i].ReleaseAction();
                }

                break;
            }

            yield return null;
        }

        StartCoroutine(ActMain());
    }
    
    IEnumerator ActMain()
    {
        yield return null;

        StartMainGame();
    }

    void StartMainGame()
    {
        stage.SetControlStatus(StageControlScript.ControlStatus.MainGame_Enabled);
        complete = true;
    }
}
