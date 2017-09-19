using UnityEngine;
using System.Collections;
using System.Collections.Generic; // List 사용을 위한 구문
using System.Xml; // XML 사용을 위한 구문

public class GenericVariableManager : MonoBehaviour
{
    // GVM : 일반 변수 매니저
    // 전역 변수 데이터를 저장하고, 호출합니다.

    // 전역 변수란 하나의 특정 객체에 할당되지 않는 게임 내의 모든 변수를 지칭합니다.
    // 중요. 특정한 객체에만 할당되는 변수는 해당 객체의 기초, 혹은 매니저 클래스에서 저장합니다!

    // ------------------------------------------------------------------------- //

    // 기본 초기화 여부 (initiated)
    bool initiated;

    // 현재 게임의 버전! *빌드인덱스로...
    int currentBuild;

    // 해당 버전에 대한 공지 여부
    bool noticed;

    // 돈 (인게임 머니, 과금 아이템이 있으면 그것도)
    int iGold; //골드, 기본 게임내 돈

    // 환경 설정값 (음악의 ON / OFF, 볼륨)
    bool bgmEnabled;
    float bgmVolume; //여기까지 브금설정

    bool sfxEnabled;
    float sfxVolume; //여기까지 효과음설정

    // 게임의 진척도 (최고 몇 스테이지까지 갔어요?)
    int gameProgress;

    // 챕터 및 게임의 규모 구성 (한 챕터 당 스테이지 수, 엔딩까지 필요한 스테이지 수)
    
    private static GenericVariableManager _instance;
    public static GenericVariableManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("Generic Settings");
                _instance = obj.AddComponent<GenericVariableManager>();
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

        // 1. (옵션) 초기화 여부를 확인하고, 필요하면 데이터를 리셋한다
        // 2. 저장한 데이터를 불러온다 (XML / PlayerPref)
    }

    //데이터를 저장, 호출하는 함수를 아래에 작성합니다.
}