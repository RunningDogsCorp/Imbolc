using UnityEngine;
using System.Collections;

public class Npc_Enemy_AI : MonoBehaviour {

    public Npc_BaseScript actorBase;
    public Npc_ActionScript actorSelf;

    public PlayerActionScript target_player;
    public Npc_ActionScript[] target_NPCs;

    bool AI_Enabled;
    int actionNumberInRandom;
    
    public enum AI_Sequence_Type
    {
        sequence_none,
        sequence_aggresive,
        sequence_defensive,
        sequence_evasive,
    }

    public AI_Sequence_Type AI_sequence;

    bool action_isLoop;
    bool action_onceDone;
    public bool action_ignoreHit;

    float distanceToPlayer;
    float distanceToEngage;

    float timeToAdvanceAttack; //공격주기 변수였지만 지금은 글쿨...
    int nextAttack;
    int finalAttack;

    float timeToRecalcAIsequence;
        
	// Use this for initialization
	void Start () {
        AI_Enabled = false;
        action_isLoop = false;
        action_ignoreHit = false;
        distanceToEngage = 4.0f;
        timeToAdvanceAttack = 0;
        nextAttack = 1;
        finalAttack = 2;

        AI_sequence = AI_Sequence_Type.sequence_none;
        timeToRecalcAIsequence = 0;
    }

    public void StartSetAIsequence()
    {

        bool ignoreIncomingAttack = false;
        Npc_Enemy_AI.AI_Sequence_Type nextSeq = AI_Sequence_Type.sequence_aggresive;

        //actorBase.refBiasAggresion;
        //actorBase.refBiasInitiative;


        float currentChoice = Random.Range(0, 1);

        if (currentChoice > actorBase.refBiasInitiative)
        {
            ignoreIncomingAttack = true;
        }
        if (ignoreIncomingAttack) return;


        currentChoice = Random.Range(0, 1);

        if (currentChoice > actorBase.refBiasAggresion)
        {
            int intBias = Random.Range(0, 2);

            switch (intBias)
            {
                case 0:
                    nextSeq = Npc_Enemy_AI.AI_Sequence_Type.sequence_defensive;
                    break;

                case 1:
                    nextSeq = Npc_Enemy_AI.AI_Sequence_Type.sequence_evasive;
                    break;
            }
        }
        else
        {
            int intBias = Random.Range(0, 2);

            switch (intBias)
            {
                case 0:
                    nextSeq = Npc_Enemy_AI.AI_Sequence_Type.sequence_defensive;
                    break;

                case 1:
                    ignoreIncomingAttack = true;
                    break;
            }
        }

        float timeReg = 1.0f;
        
        if (!ignoreIncomingAttack) SetAIsequence(nextSeq, timeReg);
    }

    void SetAIsequence(AI_Sequence_Type sequence, float timeLength = 1.0f)
    {
        AI_sequence = sequence;
        timeToRecalcAIsequence = timeLength;
    }

    void ExecuteNextAttack()
    {
        actorBase.Action = Npc_BaseScript.ACTION.ATTACK_READY;
        Npc_BaseScript.ACTION nextMove = Npc_BaseScript.ACTION.NO_ACTION;
               
        while (true)
        {
            int nextOrder = Random.Range(0, actorSelf.action.Length + 1);
            Debug.Log("공격순서 : " + nextAttack + ", 공격ID : " + nextOrder);

            if (nextOrder == actorSelf.action.Length)
            {
                int defensiveMoveCall = Random.Range(2, 4);
                
                SetAIsequence((AI_Sequence_Type)defensiveMoveCall, 0.5f);

                Debug.Log(AI_sequence);
                break;
            }

            if (nextAttack != actorSelf.action[nextOrder].actOrder)
            {
                continue;
            }

            if (actorSelf.action[nextOrder].currentCooltime <= 0)
            {
                nextMove = actorSelf.action[nextOrder].actionID;
                actorSelf.StartAction(nextMove, false);
                
                break;
            }
            else
            {
                continue;
            }
        }

        nextAttack++;
        if (nextAttack > finalAttack) nextAttack = 1;

        timeToAdvanceAttack = 1.5f;
    }

    // Update is called once per frame
    void Update () {

        if (target_NPCs.Length == 0 || target_NPCs[0] == null)
        {
            actorSelf.isFacingLeft = actorSelf.IsFacingLeft(transform.position, target_player.transform.position);
        }

        distanceToPlayer = Vector3.Distance(transform.position, target_player.transform.position);
        //Debug.Log(distanceToPlayer);

        if (Input.GetKeyDown(KeyCode.F4))
        {
            AI_Enabled = !AI_Enabled;
            AI_sequence = AI_Sequence_Type.sequence_aggresive;

            //디버그 : 에너미 속도저하
            Npc_BaseScript actorBase = GetComponent<Npc_BaseScript>();
            actorBase.refSpeed *= 0.4f;
        }

        if (AI_Enabled)
        {
            if (AI_sequence == AI_Sequence_Type.sequence_aggresive)
            {
                if (action_ignoreHit)
                {
                    timeToRecalcAIsequence -= Time.deltaTime;

                    if (timeToRecalcAIsequence < 0)
                    {
                        timeToRecalcAIsequence = 0;
                        action_ignoreHit = false;
                    }
                }

                if (distanceToPlayer < distanceToEngage)
                {
                    if (timeToAdvanceAttack <= 0)
                    {
                        ExecuteNextAttack();

                        /*
                        if (nextAttack == 1)
                        {
                            actorSelf.StartAction(Npc_BaseScript.ACTION.ATTACK_1, false);

                            timeToAdvanceAttack = 1.0f;
                            ++nextAttack;

                            Debug.Log("AI : Engaging attack 1");
                        }
                        else if (nextAttack == 2)
                        {
                            actorSelf.StartAction(Npc_BaseScript.ACTION.ATTACK_2, false);

                            timeToAdvanceAttack = 1.0f;
                            nextAttack = 1; //마지막 공격 후에 되돌아감

                            Debug.Log("AI : Engaging attack 2");
                        }
                        */
                    }
                    else
                    {
                        timeToAdvanceAttack -= Time.deltaTime;
                    }

                    //Debug.Log(timeToAdvanceAttack);
                }

                if (distanceToPlayer >= distanceToEngage)
                {
                    if (timeToAdvanceAttack != 0)
                        timeToAdvanceAttack = 0;

                    if (nextAttack != 1)
                        nextAttack = 1;

                    actorSelf.MoveAction(Npc_BaseScript.ACTION.MOVE_FORWARD);

                    Debug.Log("AI : approaching");
                }
            }

            if (AI_sequence == AI_Sequence_Type.sequence_evasive)
            {
                if (!action_onceDone)
                {
                    actorSelf.StartAction(Npc_BaseScript.ACTION.EVADE_ROLL, true);
                    action_onceDone = true;
                }
                else
                {
                    timeToRecalcAIsequence -= Time.deltaTime;

                    if (timeToRecalcAIsequence < 0)
                    {
                        AI_sequence = AI_Sequence_Type.sequence_aggresive;
                        timeToRecalcAIsequence = 0;
                        action_onceDone = false;
                    }

                    if (actorBase.Action == Npc_BaseScript.ACTION.IDLE)
                    {
                        int nextSeq = Random.Range(0, 3);
                        switch (nextSeq)
                        {
                            case 0:
                                AI_sequence = AI_Sequence_Type.sequence_aggresive;
                                timeToRecalcAIsequence = 1.0f;
                                action_onceDone = false;
                                action_ignoreHit = true;
                                break;
                            case 1:
                                AI_sequence = AI_Sequence_Type.sequence_defensive;
                                action_onceDone = false;
                                break;
                            case 2:
                                actorSelf.MoveAction(Npc_BaseScript.ACTION.MOVE_BACK); //뒤로 빠지기: 액션 들어감
                                break;
                        }
                    }

                    if (actorBase.Action == Npc_BaseScript.ACTION.MOVE_BACK)
                    {
                        actorSelf.MoveAction(Npc_BaseScript.ACTION.MOVE_BACK); //뒤로 빠지기: 액션 유지
                        Debug.Log("Evade: retreating");
                    }
                }
            }

            if (AI_sequence == AI_Sequence_Type.sequence_defensive)
            {
                timeToRecalcAIsequence -= Time.deltaTime;

                if (timeToRecalcAIsequence < 0)
                {
                    AI_sequence = AI_Sequence_Type.sequence_aggresive;
                    timeToRecalcAIsequence = 0;
                }

                actorSelf.MoveAction(Npc_BaseScript.ACTION.BLOCK);
            }

        }

	}
}
