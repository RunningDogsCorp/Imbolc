using UnityEngine;
using System.Collections;

public class Npc_ActionScript : MonoBehaviour {

    Npc_BaseScript npcBase;
    bool loopAction;
    
    public Npc_HitBoxScript hitBox;

    public bool isFacingLeft;
    //public 애니메이터 등록

    public struct Action
    {
        public Npc_BaseScript.ACTION actionID;
        public int actOrder;
        public float actionCooltime;
        public float currentCooltime;
    }

    public Action[] action;

    void ApplyActionPool()
    {
        action = new Action[5];

        action[0].actionID = Npc_BaseScript.ACTION.ATTACK_HIGH;
        action[0].actOrder = 1;
        action[0].actionCooltime = 1.0f;

        action[1].actionID = Npc_BaseScript.ACTION.ATTACK_HIGH_2;
        action[1].actOrder = 2;
        action[1].actionCooltime = 0.5f;

        action[2].actionID = Npc_BaseScript.ACTION.ATTACK_MIDDLE;
        action[2].actOrder = 1;
        action[2].actionCooltime = 1.0f;

        action[3].actionID = Npc_BaseScript.ACTION.ATTACK_MIDDLE_2;
        action[3].actOrder = 2;
        action[3].actionCooltime = 1.5f;

        action[4].actionID = Npc_BaseScript.ACTION.ATTACK_SPECIAL;
        action[4].actOrder = 1;
        action[4].actionCooltime = 4;

        for (int i = 0; i < action.Length; ++i)
        {
            action[i].currentCooltime = 0;
        }
    }

    // Use this for initialization
    void Start () {
        npcBase = GetComponent<Npc_BaseScript>();
        loopAction = false;

        if (npcBase.NpcType == Npc_BaseScript.NPC_TYPE.ENEMY)
        {
            isFacingLeft = true;
        }
        else isFacingLeft = false;

        ApplyActionPool();
    }

    public bool IsFacingLeft(Vector3 positionSelf, Vector3 positionTarget)
    {
        if (positionSelf.x > positionTarget.x)
        {
            return true;
        }
        else if (positionSelf.x < positionTarget.x)
        {
            return false;
        }
        else
        {
            return isFacingLeft;
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < action.Length; ++i)
        {
            if (action[i].currentCooltime > 0) action[i].currentCooltime -= Time.deltaTime;
            else action[i].currentCooltime = 0;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown("1"))
            {
                StartAction(Npc_BaseScript.ACTION.IDLE, false);
            }

            if (Input.GetKeyDown("2"))
            {
                StartAction(Npc_BaseScript.ACTION.MOVE_FORWARD, true);
            }

            if (Input.GetKeyUp("2"))
            {
                ReleaseAction();
            }

            if (Input.GetKeyDown("3"))
            {
                StartAction(Npc_BaseScript.ACTION.MOVE_BACK, true);
            }

            if (Input.GetKeyUp("3"))
            {
                ReleaseAction();
            }

            if (Input.GetKeyDown("4"))
            {
                StartAction(Npc_BaseScript.ACTION.MOVE_FAST_FORWARD, true);
            }

            if (Input.GetKeyUp("4"))
            {
                ReleaseAction();
            }

            if (Input.GetKeyDown("5"))
            {
                StartAction(Npc_BaseScript.ACTION.MOVE_FAST_BACK, true);
            }

            if (Input.GetKeyUp("5"))
            {
                ReleaseAction();
            }

            if (Input.GetKeyDown("a"))
            {
                StartAction(Npc_BaseScript.ACTION.ATTACK_1, false);
            }

            if (Input.GetKeyDown("s"))
            {
                StartAction(Npc_BaseScript.ACTION.ATTACK_1_RETURN, false);
            }

            if (Input.GetKeyDown("d"))
            {
                StartAction(Npc_BaseScript.ACTION.ATTACK_2, false);
            }

            if (Input.GetKeyDown("f"))
            {
                StartAction(Npc_BaseScript.ACTION.ATTACK_2_RETURN, false);
            }

            if (Input.GetKeyDown("e"))
            {
                StartAction(Npc_BaseScript.ACTION.EVADE_ROLL, true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopAction();
            }
        }
    }

    public void StartAction(Npc_BaseScript.ACTION action, bool loop)
    {
        npcBase.Action = action;
        loopAction = loop;
        ManageAction();
    }

    //업데이트 (홀드 입력 대응)
    public void MoveAction(Npc_BaseScript.ACTION action)
    {
        if (npcBase.Action != action)
        {
            //액션 지정
            npcBase.Action = action;

            //애니메이션 적용 (1회면 되니까)
        }

        ManageMove();
    }

    public void StopAction()
    {
        loopAction = false;
    }

    public void ReleaseAction()
    {
        StopAction();
        StartAction(Npc_BaseScript.ACTION.IDLE, false);
    }

    void ManageAction()
    {
        switch (npcBase.Action)
        {
            case Npc_BaseScript.ACTION.IDLE:
                StartCoroutine(Action_Idle());
                break;

            case Npc_BaseScript.ACTION.MOVE_FORWARD:
                StartCoroutine(Action_Move_Forward());
                break;

            case Npc_BaseScript.ACTION.MOVE_BACK:
                StartCoroutine(Action_Move_Back());
                break;

            case Npc_BaseScript.ACTION.MOVE_FAST_FORWARD:
                StartCoroutine(Action_Move_Fast_Forward());
                break;

            case Npc_BaseScript.ACTION.MOVE_FAST_BACK:
                StartCoroutine(Action_Move_Fast_Back());
                break;

            case Npc_BaseScript.ACTION.ATTACK_1:
                StartCoroutine(Action_Attack_1());
                break;

            case Npc_BaseScript.ACTION.ATTACK_1_RETURN:
                StartCoroutine(Action_Attack_1_Return());
                break;

            case Npc_BaseScript.ACTION.ATTACK_2:
                StartCoroutine(Action_Attack_2());
                break;

            case Npc_BaseScript.ACTION.ATTACK_2_RETURN:
                StartCoroutine(Action_Attack_2_Return());
                break;

            case Npc_BaseScript.ACTION.ATTACK_3:
                break;

            case Npc_BaseScript.ACTION.ATTACK_3_RETURN:
                break;

            case Npc_BaseScript.ACTION.ATTACK_4:
                break;

            case Npc_BaseScript.ACTION.ATTACK_4_RETURN:
                break;

            case Npc_BaseScript.ACTION.ATTACK_5:
                break;

            case Npc_BaseScript.ACTION.ATTACK_5_RETURN:
                break;

            case Npc_BaseScript.ACTION.BOW_DRAG:
                break;

            case Npc_BaseScript.ACTION.BOW_HOLD:
                break;

            case Npc_BaseScript.ACTION.BOW_SHOOT:
                break;

            case Npc_BaseScript.ACTION.BOW_RELOAD:
                break;

            case Npc_BaseScript.ACTION.BOW_RETURN:
                break;

            case Npc_BaseScript.ACTION.BLOCK:
                break;

            case Npc_BaseScript.ACTION.BLOCK_ENTER:
                break;

            case Npc_BaseScript.ACTION.BLOCK_OUT:
                break;

            case Npc_BaseScript.ACTION.BLOCK_HIT:
                break;

            case Npc_BaseScript.ACTION.BLOCK_HIT_RECOVER:
                break;

            case Npc_BaseScript.ACTION.BLOCK_HIT_HEAVY:
                break;

            case Npc_BaseScript.ACTION.BLOCK_HIT_HEAVY_RECOVER:
                break;

            case Npc_BaseScript.ACTION.BLOCK_HIT_HOLDBACK:
                break;

            case Npc_BaseScript.ACTION.BLOCK_HIT_HOLDBACK_RECOVER:
                break;

            case Npc_BaseScript.ACTION.BLOCK_REVENGE:
                break;

            case Npc_BaseScript.ACTION.HIT:
                break;

            case Npc_BaseScript.ACTION.HIT_RECOVER:
                break;

            case Npc_BaseScript.ACTION.EVADE_ROLL:
                StartCoroutine(Action_Evade_Roll());
                break;

            case Npc_BaseScript.ACTION.EVADE_ROLL_OUT:
                break;

            case Npc_BaseScript.ACTION.KNOCKED:
                break;

            case Npc_BaseScript.ACTION.DEATH:
                break;

            case Npc_BaseScript.ACTION.ATTACK_HIGH:
                StartCoroutine(Action_Attack_HIGH());
                break;

            case Npc_BaseScript.ACTION.ATTACK_HIGH_2:
                StartCoroutine(Action_Attack_HIGH_2());
                break;

            case Npc_BaseScript.ACTION.ATTACK_LOW:
                break;

            case Npc_BaseScript.ACTION.ATTACK_LOW_2:
                break;

            case Npc_BaseScript.ACTION.ATTACK_MIDDLE:
                StartCoroutine(Action_Attack_MID());
                break;

            case Npc_BaseScript.ACTION.ATTACK_MIDDLE_2:
                StartCoroutine(Action_Attack_MID_2());
                break;

            case Npc_BaseScript.ACTION.ATTACK_READY:
                break;

            case Npc_BaseScript.ACTION.ATTACK_SPECIAL:
                StartCoroutine(Action_Attack_SP());
                break;

            case Npc_BaseScript.ACTION.ATTACK_SPECIAL_2:
                break;
        }
    }

    void ManageMove()
    {
        switch (npcBase.Action)
        {
            case Npc_BaseScript.ACTION.IDLE:
                Move_Idle();
                break;

            case Npc_BaseScript.ACTION.MOVE_FORWARD:
                Move_Forward();
                break;

            case Npc_BaseScript.ACTION.MOVE_BACK:
                Move_Back();
                break;

            case Npc_BaseScript.ACTION.MOVE_FAST_FORWARD:
                Move_Fast_Forward();
                break;

            case Npc_BaseScript.ACTION.MOVE_FAST_BACK:
                Move_Fast_Back();
                break;

            case Npc_BaseScript.ACTION.BLOCK:
                Move_Block();
                break;
        }
    }

    //업데이트용 개별액션

    void Move_Idle()
    {
        //아이들이 필요할지도 모르니 일단 만들어둠니다
    }

    void Move_Forward()
    {
        if (isFacingLeft)
        {
            transform.Translate(-npcBase.refSpeed, 0, 0);
        }
        else
        {
            transform.Translate(npcBase.refSpeed, 0, 0);
        }
    }

    void Move_Back()
    {
        if (isFacingLeft)
        {
            transform.Translate(npcBase.refSpeed, 0, 0);
        }
        else
        {
            transform.Translate(-npcBase.refSpeed, 0, 0);
        }
    }

    void Move_Fast_Forward()
    {
        if (isFacingLeft)
        {
            transform.Translate(-npcBase.refSpeed * 2, 0, 0);
        }
        else
        {
            transform.Translate(npcBase.refSpeed * 2, 0, 0);
        }
    }

    void Move_Fast_Back()
    {
        if (isFacingLeft)
        {
            transform.Translate(npcBase.refSpeed * 2, 0, 0);
        }
        else
        {
            transform.Translate(-npcBase.refSpeed * 2, 0, 0);
        }
    }

    void Move_Block()
    {
        Debug.Log("blocking!");
    }

    //코루틴용 개별액션

    IEnumerator Action_Idle()
    {
        Debug.Log(npcBase.Action);

        yield return null;
    }

    IEnumerator Action_Evade_Roll()
    {
        Debug.Log("Evasion : roll!");

        float time = 0;

        while (loopAction)
        {
            if (time > 0.15f)
            {
                ReleaseAction();
                break;
            }

            transform.Translate(npcBase.refSpeed * 3, 0, 0);
            time += Time.deltaTime;
            yield return null;
        }
        
    }

    IEnumerator Action_Block()
    {
        Debug.Log("Blocking : in coroutine!");

        yield return null;
    }


    IEnumerator Action_Move_Forward()
    {
        Debug.Log(npcBase.Action);

        while (loopAction)
        {
            if (npcBase.Action != Npc_BaseScript.ACTION.MOVE_FORWARD) break;

            if (isFacingLeft)
            {
                transform.Translate(-npcBase.refSpeed, 0, 0);
            }
            else
            {
                transform.Translate(npcBase.refSpeed, 0, 0);
            }
            yield return null;
        }
    }

    IEnumerator Action_Move_Back()
    {
        Debug.Log(npcBase.Action);

        while (loopAction)
        {
            if (npcBase.Action != Npc_BaseScript.ACTION.MOVE_BACK) break;

            if (isFacingLeft)
            {
                transform.Translate(npcBase.refSpeed, 0, 0);
            }
            else
            {
                transform.Translate(-npcBase.refSpeed, 0, 0);
            }
            yield return null;
        }
    }

    IEnumerator Action_Move_Fast_Forward()
    {
        Debug.Log(npcBase.Action);

        while (loopAction)
        {
            if (npcBase.Action != Npc_BaseScript.ACTION.MOVE_FAST_FORWARD) break;

            if (isFacingLeft)
            {
                transform.Translate(-npcBase.refSpeed * 2, 0, 0);
            }
            else
            {
                transform.Translate(npcBase.refSpeed * 2, 0, 0);
            }
            yield return null;
        }
    }

    IEnumerator Action_Move_Fast_Back()
    {
        Debug.Log(npcBase.Action);

        while (loopAction)
        {
            if (npcBase.Action != Npc_BaseScript.ACTION.MOVE_FAST_BACK) break;

            if (isFacingLeft)
            {
                transform.Translate(npcBase.refSpeed * 2, 0, 0);
            }
            else
            {
                transform.Translate(-npcBase.refSpeed * 2, 0, 0);
            }
            yield return null;
        }
    }

    IEnumerator Action_Attack_1()
    {
        Debug.Log(npcBase.Action);

        hitBox.gameObject.SetActive(true);
        hitBox.Initiate(0.5f);

        yield return null;
    }

    IEnumerator Action_Attack_1_Return()
    {
        Debug.Log(npcBase.Action);

        yield return null;
    }

    IEnumerator Action_Attack_2()
    {
        Debug.Log(npcBase.Action);

        hitBox.gameObject.SetActive(true);
        hitBox.Initiate(0.5f);

        yield return null;
    }

    IEnumerator Action_Attack_2_Return()
    {
        Debug.Log(npcBase.Action);

        yield return null;
    }

    IEnumerator Action_Attack_HIGH()
    {
        Debug.Log(npcBase.Action);

        hitBox.gameObject.SetActive(true);
        hitBox.Initiate(0.5f);

        action[0].currentCooltime = action[0].actionCooltime;

        yield return null;
    }

    IEnumerator Action_Attack_HIGH_2()
    {
        Debug.Log(npcBase.Action);

        hitBox.gameObject.SetActive(true);
        hitBox.Initiate(0.5f);

        action[1].currentCooltime = action[1].actionCooltime;

        yield return null;
    }

    IEnumerator Action_Attack_MID()
    {
        Debug.Log(npcBase.Action);

        hitBox.gameObject.SetActive(true);
        hitBox.Initiate(0.5f);

        action[2].currentCooltime = action[2].actionCooltime;

        yield return null;
    }

    IEnumerator Action_Attack_MID_2()
    {
        Debug.Log(npcBase.Action);

        hitBox.gameObject.SetActive(true);
        hitBox.Initiate(0.5f);

        action[3].currentCooltime = action[3].actionCooltime;

        yield return null;
    }

    IEnumerator Action_Attack_SP()
    {
        Debug.Log(npcBase.Action);

        hitBox.gameObject.SetActive(true);
        hitBox.Initiate(0.5f);

        action[4].currentCooltime = action[4].actionCooltime;

        yield return null;
    }
}
