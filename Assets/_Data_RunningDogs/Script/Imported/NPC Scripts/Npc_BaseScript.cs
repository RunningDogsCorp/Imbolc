using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class Npc_BaseScript : MonoBehaviour
{

    Npc_ActionScript actionData;

    //npc 기본타입. 적인가, 아군인가?
    public enum NPC_TYPE
    {
        FRIENDLY = 0, // 아군, 혹은 다른 무엇인가
        ENEMY, // 적
    }

    //타입 구분 삭제 예정. 모든 클래스를 별도의 파일로 나눌 계획.
    //적일 경우의 타입. 어떤 적인가?
    //행동유형 및 전투거리로 타입분화 필요
    //단순 사정거리 분류는 AI구분이 귀찮고 무성의해 폐기했음.

    public enum ENEMY_TYPE
    {
        NOT_ENEMY = 0, // 적이 아닙니다!
        CHARGER, // 초근접형. 닥돌러
        BRUTE, // 근접형, 파워형
        POKER, // 중장거리형, 견제형
        ARCHER, // 장거리형, 원딜러
        //필요하면 새로 추가
    }

    // 액션 및 상태 (enum 작성)
    public enum ACTION
    {
        NO_ACTION,

        IDLE,

        MOVE_FORWARD,
        MOVE_BACK,
        MOVE_FAST_FORWARD,
        MOVE_FAST_BACK,

        //근접공격 (기술화)
        //공격준비 (공격을 하기 위한 전단계)
        ATTACK_READY,
        //일반공격 (쿨타임 짧음)
        ATTACK_HIGH, //상단
        ATTACK_MIDDLE, //중단
        ATTACK_LOW, //하단
        //연속공격 (쿨타임 짧음)
        ATTACK_HIGH_2, //상단
        ATTACK_MIDDLE_2, //중단
        ATTACK_LOW_2, //하단
        //특수공격 (쿨타임 김)
        //*몬스터 액션 테이블이 나오는 대로 분화할 것.
        //+ 오거, 데스나이트 등을 별개 클래스로
        ATTACK_SPECIAL,
        ATTACK_SPECIAL_2,

        //구 근접공격 (남겨놓음)
        //일반공격+회수구분용
        ATTACK_1,
        ATTACK_1_RETURN,
        ATTACK_2,
        ATTACK_2_RETURN,
        ATTACK_3,
        ATTACK_3_RETURN,
        ATTACK_4,
        ATTACK_4_RETURN,
        ATTACK_5,
        ATTACK_5_RETURN,
        //특수공격 (모아치기, 마법주문 등이 있다면)
        ATTACK_SP_1_READY, //준비단계 (보고 피하라)
        ATTACK_SP_1_ACTION, //실행단계 (실제 행동)
        ATTACK_SP_1_RETURN, //회수단계 (빈틈)
        ATTACK_SP_2_READY,
        ATTACK_SP_2_ACTION,
        ATTACK_SP_2_RETURN,
        ATTACK_SP_3_READY,
        ATTACK_SP_3_ACTION,
        ATTACK_SP_3_RETURN,

        //원거리 공격 가능한 적이 있다면
        BOW_DRAG,
        BOW_HOLD,
        BOW_SHOOT,
        BOW_RELOAD,
        BOW_RETURN,

        BLOCK,
        BLOCK_ENTER,
        BLOCK_OUT,

        BLOCK_HIT,
        BLOCK_HIT_RECOVER,
        BLOCK_HIT_HEAVY,
        BLOCK_HIT_HEAVY_RECOVER,
        BLOCK_HIT_HOLDBACK,
        BLOCK_HIT_HOLDBACK_RECOVER,
        BLOCK_REVENGE,

        HIT,
        HIT_RECOVER,

        EVADE_ROLL,
        EVADE_ROLL_OUT,

        EVADE_ROLL_FORWARD,
        EVADE_ROLL_FORWARD_OUT,

        KNOCKED,
        DEATH,
    }

    public enum STATE
    {
        NORMAL,
        RAGED,
        GOTFIRE,
        BLEED,
        DIZZY,
        SLOWED,
        KNOCKED,
        READY_TO_FINISH, //사망 직전 상태. 피니시 가능
        DEATH,
    }

    ACTION action;
    STATE state;

    //<XML 1차 파싱>
    //NPC 기본 정보 : 파싱도 하지만, 인덱스와 타입은 인스펙터에서도 제어할 수 있도록 한다!
    // (=인스펙터에서 프리팹 설정값을 지정 + 파싱에서 검별후 데이터 적용)
    
    public int npcIndex; //NPC 인덱스
    string npcName; //NPC 이름

    NPC_TYPE npcType; //NPC의 유형, 적인가, 아군인가?
    ENEMY_TYPE enemyType; //NPC의 적 유형 (어떤 적인가?)

    //기본 스테이터스
    float baseHP;
    float baseGuard;

    float baseAttackMelee;
    float baseAttackRange;

    float baseDefence;

    float baseCriticalChance;
    float baseCriticalRate;

    float baseSpeed;
    float baseEvadeTime;
    float baseKnockTime;
    float baseBlockHoldTime;
    float baseRangeOfMelee;
    
    //삭제예정 코드 : 성격편향
    float baseBiasAggresion;
    float baseBiasInitiative;
    
    //</XML 1차 파싱>

    //<XML 2차 파싱>

    //각종 2차 스테이터스 그룹을 여기에....
    float baseSufferCriticalChance;
    float baseSufferCriticalRate;

    //</XML 2차 파싱>

    //공격력, 방어력 순간 보정값 (타입과 액션에 따라 달라짐)
    float offsetAttack;
    float effsetDefence;
    
    //현재 상태 (변동값. 파싱하지 않는다)
    float currentHP;
    float currentGuard;

    //파워레벨 총보정값 (진척도에 따라. 파싱여부는 아직 미결정. 인스펙터 접근가능)
    public float offsetPowerLevel;

    //변환된 스테이터스값 (현재 이쪽이 최종 스테이터스일 예정)
    public float refHP;
    public float refGuard;

    public float refAttackMelee;
    public float refAttackRange;

    public float refDefence;

    public float refCriticalRate;
    public float refCriticalChance;

    public float refSpeed;
    public float refEvadeTime;
    public float refKnockTime;
    public float refBlockHoldTime;
    public float refRangeOfMelee;

    public float refBiasAggresion;
    public float refBiasInitiative;

    public NPC_TYPE NpcType
    {
        get
        {
            return npcType;
        }
    }

    private void Start()
    {
        actionData = GetComponent<Npc_ActionScript>();

        ParseBaseDataFromXML();

        CalcRefStatus();

        action = ACTION.IDLE;
        state = STATE.NORMAL;

        //디버그 코드 : XML에 차후 포함시켜야 합니다.
        baseBiasAggresion = 1.0f;
        baseBiasInitiative = 1.0f;
        refBiasAggresion = baseBiasAggresion;
        refBiasInitiative = baseBiasInitiative;
    }

    public ACTION Action
    {
        get { return action; }
        set { action = value; }
    }

    public STATE State
    {
        get { return state; }
        set { state = value; }
    }

    void ParseBaseDataFromXML()
    {
        TextAsset xml = Resources.Load<TextAsset>("XML/NPCBase");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml.text);

        XmlElement tempDoc = doc["NPCBaseData"];

        //데이터 노드와 세부요소
        XmlNode dataNode = tempDoc.FirstChild;
        XmlElement data;

        for (int i = 0; i < tempDoc.ChildNodes.Count; ++i, dataNode = dataNode.NextSibling)
        {
            data = (XmlElement)dataNode; //각 필드에서 세부요소 추출
                                         //*For문을 반복할 때마다 NextSibling으로 노드가 한 단계씩 나아갑니다

            //인스펙터에서 할당한 인덱스를 XML과 비교합니다. (=NPC 실제 인덱스는 에디터 프리팹에서 조작)
            int index = System.Convert.ToInt16(data.GetAttribute("npcIndex"));

            //XML이 인덱스와 일치하면 나머지 데이터를 추출해서 적용합니다.
            if (index == npcIndex)
            {
                npcName = data.GetAttribute("npcName");

                npcType = (NPC_TYPE)System.Convert.ToInt16(data.GetAttribute("npcType"));
                enemyType = (ENEMY_TYPE)System.Convert.ToInt16(data.GetAttribute("enemyType"));

                baseHP = System.Convert.ToSingle(data.GetAttribute("baseHP"));
                baseGuard = System.Convert.ToSingle(data.GetAttribute("baseGuard"));

                baseAttackMelee = System.Convert.ToSingle(data.GetAttribute("baseAttackMelee"));
                baseAttackRange = System.Convert.ToSingle(data.GetAttribute("baseAttackRange"));

                baseDefence = System.Convert.ToSingle(data.GetAttribute("baseDefence"));

                baseCriticalChance = System.Convert.ToSingle(data.GetAttribute("baseCriticalChance"));
                baseCriticalRate = System.Convert.ToSingle(data.GetAttribute("baseCriticalRate"));

                baseSpeed = System.Convert.ToSingle(data.GetAttribute("baseSpeed"));
                baseEvadeTime = System.Convert.ToSingle(data.GetAttribute("baseEvadeTime"));
                baseKnockTime = System.Convert.ToSingle(data.GetAttribute("baseKnockTime"));
                baseBlockHoldTime = System.Convert.ToSingle(data.GetAttribute("baseBlockHoldTime"));
                baseRangeOfMelee = System.Convert.ToSingle(data.GetAttribute("baseRangeOfMelee"));

                //데이터를 적용했으니 나머지는 더 볼 일 없이 바로 브레이크
                break;
            }
        }

    }

    void CalcRefStatus()
    {
        refHP = baseHP;
        refGuard = baseGuard;

        refAttackMelee = baseAttackMelee;
        refAttackRange = baseAttackRange;

        refDefence = baseDefence;

        refCriticalChance = baseCriticalChance;
        refCriticalRate = baseCriticalRate;

        refSpeed = baseSpeed;
        refEvadeTime = baseEvadeTime;
        refKnockTime = baseKnockTime;
        refBlockHoldTime = baseBlockHoldTime;
        refRangeOfMelee = baseRangeOfMelee;
        
    }
    
}
