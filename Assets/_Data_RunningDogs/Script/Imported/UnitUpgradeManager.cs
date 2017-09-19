using UnityEngine;
using System.Collections;
using System.Xml; // XML 사용을 위한 구문

public class UnitUpgradeManager : MonoBehaviour {

    //플레이어 유닛(단위) 업그레이드 값들을 관리, 호출하는 싱글톤
    //테이블 방식으로 바뀌면 그 때 가서 교체해야

    //체력, 가드게이지
    public float unitHP;
    public float unitGuard;

    //공격력, 방어력
    public float unitAttackMelee;
    public float unitAttackRange;
    public float unitDefence;

    //방어, 회피, 경직 관련
    public float unitEvadeTime;
    public float unitKnockTime;
    public float unitBlockHoldTime;

    //치명타 방어
    public float unitCriticalBlockChance;
    public float unitCriticalBlockRate;

    //치명타 공격
    public float unitCriticalAttackChance;
    public float unitCriticalAttackRate;

    private static UnitUpgradeManager _instance;
    public static UnitUpgradeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("Generic Settings");
                _instance = obj.AddComponent<UnitUpgradeManager>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        InitiateDataFromXML();
    }

    void InitiateDataFromXML()
    {
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
        }
    }

    public void ShowViaConsole()
    {
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
}
