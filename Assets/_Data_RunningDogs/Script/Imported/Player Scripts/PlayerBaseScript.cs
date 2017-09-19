using UnityEngine;
using System.Collections;
using System.Xml; // XML 사용을 위한 구문

public class PlayerBaseScript : MonoBehaviour {

    // 플레이어 기초 클래스. 프리팹 내 제어를 목적으로 합니다.

    // 구체적인 변수 선언 및 할당 방식은 미확정이며, XML에 대응해야 할 수 있습니다.
    // 17. 3. 14. : 플레이어에 해당되는 모든 변수는 이쪽으로 할당합니다.
    // 단, 변수 작성 원칙은 같습니다. 최종값이 아닌 단위값 위주로 작성해야 합니다.

    // -------------------------------------------------------------------- //

    PlayerActionScript actionData;
    
    // 액션 및 상태 (enum 작성)
    public enum ACTION
    {
        NO_ACTION,

        IDLE,

        MOVE_LEFT,
        MOVE_RIGHT,
        MOVE_FAST_LEFT,
        MOVE_FAST_RIGHT,

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

        EVADE_ROLL_FORWARD,
        EVADE_ROLL_FORWARD_OUT,
        EVADE_ROLL_BACKWARD,
        EVADE_ROLL_BACKWARD_OUT,

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
        DEATH,
    }

    ACTION action;
    STATE state;

    //<XML 1차 파싱>
    //플레이어 기본 정보 : 파싱도 하지만, 인스펙터에서도 제어할 수 있도록 한다!
    // (=인스펙터에서 프리팹 인덱스를 지정 + 파싱에서 검별후 데이터 적용)
    public int characterIndex; //플레이어 캐릭터 인덱스
    string characterName; //플레이어 캐릭터 이름

    //기본 스테이터스 (업그레이드 레벨 0인 상태)
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

    // 스테이터스별 레벨 (저장 및 호출. 파싱하지 않는다)
    //체력, 가드게이지
    int levelHP;
    int levelGuard;

    //공격력, 방어력
    int levelAttackMelee;
    int levelAttackRange;
    int levelDefence;

    //방어, 회피, 경직 관련
    int levelEvadeTime;
    int levelKnockTime;
    int levelBlockHoldTime;

    //치명타 방어
    int levelCriticalBlockChance;
    int levelCriticalBlockRate;

    //치명타 공격
    int levelCriticalAttackChance;
    int levelCriticalAttackRate;

    //<XML 3차 파싱>
    // 각 스테이터스별 업그레이드 단위값
    //체력, 가드게이지
    float unitHP;
    float unitGuard;

    //공격력, 방어력
    float unitAttackMelee;
    float unitAttackRange;
    float unitDefence;

    //방어, 회피, 경직 관련
    float unitEvadeTime;
    float unitKnockTime;
    float unitBlockHoldTime;

    //치명타 방어
    float unitCriticalBlockChance;
    float unitCriticalBlockRate;

    //치명타 공격
    float unitCriticalAttackChance;
    float unitCriticalAttackRate;
    //</XML 3차 파싱>

    // 아이템 슬롯 (인벤토리가 있을 경우)
    int itemWeaponMelee;
    int itemWeaponRange;
    int itemArmor;
    int itemHelmet;
    int itemScarf;
    int itemShield;
    
    // 현재 / 기본 장착 아이템 (인벤토리가 있을 경우)
    int defaultWeaponMelee;
    int defaultWeaponRange;
    int defaultArmor;
    int defaultHelmet;
    int defaultScarf;
    int defaultShield;

    public float refHP;
    public float refGuard;

    public float refAttackMelee;
    public float refAttackRange;

    public float refDefence;

    public float refCriticalChance;
    public float refCriticalRate;

    public float refSpeed;
    public float refEvadeTime;
    public float refKnockTime;
    public float refBlockHoldTime;
    public float refRangeOfMelee;

    // Use this for initialization
    void Start () {
        actionData = GetComponent<PlayerActionScript>();

        //디폴트 아이템 ID 지정
        SetDefaultItem();

        //파싱
        ParseBaseDataFromXML();
        ParseSecondaryDataFromXML();
        UnitUpgradeManager.Instance.ShowViaConsole();

        //스탯 결과값 계산
        CalcReferenceStatus();

        action = ACTION.IDLE;
        state = STATE.NORMAL;
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
        TextAsset xml = Resources.Load<TextAsset>("XML/PlayerBase");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml.text);

        XmlElement tempDoc = doc["PlayerBaseData"];

        //데이터 노드와 세부요소
        XmlNode dataNode = tempDoc.FirstChild;
        XmlElement data;
        
        for (int i = 0; i < tempDoc.ChildNodes.Count; ++i, dataNode = dataNode.NextSibling)
        {
            data = (XmlElement)dataNode; //각 필드에서 세부요소 추출
            //*For문을 반복할 때마다 NextSibling으로 노드가 한 단계씩 나아갑니다
            
            //인스펙터에서 할당한 인덱스를 XML과 비교합니다. (=플레이어 실제 인덱스는 에디터 프리팹에서 조작)
            int index = System.Convert.ToInt16(data.GetAttribute("characterIndex"));
            
            //XML이 인덱스와 일치하면 나머지 데이터를 추출해서 적용합니다.
            if (index == characterIndex)
            {
                characterName = data.GetAttribute("characterName");

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

        //콘솔출력
        Debug.Log(characterName);
        Debug.Log(baseHP);
        Debug.Log(baseGuard);
        Debug.Log(baseAttackMelee);
        Debug.Log(baseAttackRange);
        Debug.Log(baseDefence);
        Debug.Log(baseCriticalChance);
        Debug.Log(baseCriticalRate);
        Debug.Log(baseSpeed);
        Debug.Log(baseEvadeTime);
        Debug.Log(baseKnockTime);
        Debug.Log(baseBlockHoldTime);
        Debug.Log(baseRangeOfMelee);
    }

    void ParseSecondaryDataFromXML()
    {
        TextAsset xml = Resources.Load<TextAsset>("XML/PlayerBaseSecondary");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml.text);

        XmlElement tempDoc = doc["PlayerBaseSecondaryData"];

        //데이터 노드와 세부요소
        XmlNode dataNode = tempDoc.FirstChild;
        XmlElement data;

        for (int i = 0; i < tempDoc.ChildNodes.Count; ++i, dataNode = dataNode.NextSibling)
        {
            data = (XmlElement)dataNode; //각 필드에서 세부요소 추출
                                         //*For문을 반복할 때마다 NextSibling으로 노드가 한 단계씩 나아갑니다

            //인스펙터에서 할당한 인덱스를 XML과 비교합니다. (=플레이어 실제 인덱스는 에디터 프리팹에서 조작)
            int index = System.Convert.ToInt16(data.GetAttribute("characterIndex"));

            //XML이 인덱스와 일치하면 나머지 데이터를 추출해서 적용합니다.
            if (index == characterIndex)
            {
                baseSufferCriticalChance = System.Convert.ToSingle(data.GetAttribute("baseSufferCriticalChance"));
                baseSufferCriticalRate = System.Convert.ToSingle(data.GetAttribute("baseSufferCriticalRate"));
                
                //데이터를 적용했으니 나머지는 더 볼 일 없이 바로 브레이크
                break;
            }
        }

        Debug.Log(baseSufferCriticalChance);
        Debug.Log(baseSufferCriticalRate);
    }
    
    //업그레이드 단위값 파싱 함수 (현재 사용안함. 싱글톤에서 관리중)
    void ParseUnitUpgradeDataFromXML()
    {
        TextAsset xml = Resources.Load<TextAsset>("XML/PlayerBaseUpgrade");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml.text);

        XmlElement tempDoc = doc["PlayerBaseUpgradeData"];

        //데이터 노드와 세부요소
        XmlNode dataNode = tempDoc.FirstChild;
        XmlElement data;

        data = (XmlElement)dataNode; //필드에서 세부요소 추출

        unitHP = System.Convert.ToSingle(data.GetAttribute("unitHP"));
        unitGuard = System.Convert.ToSingle(data.GetAttribute("unitGuard"));

        unitAttackMelee = System.Convert.ToSingle(data.GetAttribute("unitAttackMelee"));
        unitAttackRange = System.Convert.ToSingle(data.GetAttribute("unitAttackRange"));
        unitDefence = System.Convert.ToSingle(data.GetAttribute("unitDefence"));

        unitEvadeTime = System.Convert.ToSingle(data.GetAttribute("unitEvadeTime"));
        unitKnockTime = System.Convert.ToSingle(data.GetAttribute("unitKnockTime"));
        unitBlockHoldTime = System.Convert.ToSingle(data.GetAttribute("unitBlockHoldTime"));

        unitCriticalBlockChance = System.Convert.ToSingle(data.GetAttribute("unitCriticalBlockChance"));
        unitCriticalBlockRate = System.Convert.ToSingle(data.GetAttribute("unitCriticalBlockRate"));

        unitCriticalAttackChance = System.Convert.ToSingle(data.GetAttribute("unitCriticalAttackChance"));
        unitCriticalAttackRate = System.Convert.ToSingle(data.GetAttribute("unitCriticalAttackRate"));


        Debug.Log(unitHP);
        Debug.Log(unitGuard);

        Debug.Log(unitAttackMelee);
        Debug.Log(unitAttackRange);
        Debug.Log(unitDefence);

        Debug.Log(unitEvadeTime);
        Debug.Log(unitKnockTime);
        Debug.Log(unitBlockHoldTime);

        Debug.Log(unitCriticalBlockChance);
        Debug.Log(unitCriticalBlockRate);

        Debug.Log(unitCriticalAttackChance);
        Debug.Log(unitCriticalAttackRate);
    }

    void CalcReferenceStatus()
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

    void SetDefaultItem()
    {
        defaultWeaponMelee = 0;
        defaultWeaponRange = 0;
        defaultArmor = 0;
        defaultHelmet = 0;
        defaultScarf = 0;
        defaultShield = 0;
    }
}
