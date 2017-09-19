using UnityEngine;
using System.Collections;
//using UnityStandardAssets.CrossPlatformInput;

public class PlayerActionScript : MonoBehaviour {

    public Animator animatorSelf;

    PlayerBaseScript playerBase;

    //공격 패턴 임시 저장 변수
    public int tempAtteckCount;
    //공격 애니메이션 클립 목록
    public AnimationClip[] atkAnim;
    //걷기 애니메이션 클립 목록  0.일반 전진  1. 일반 후진  2. 방어모드 전진   3. 방어모드 후진
    public AnimationClip[] walkAnim;
    //덤블링 애니메이션 클립
    public AnimationClip[] tumbling; //앞구르기 0.일반    1.활
    public AnimationClip[] backTumbling; //뒤구르기 0.일반    1.활
    //덤블링 시 이동할 스피트값
    public float tumblingSpeed;
    //덤블링 시 이동 거리
    public float tumblingLength;
    //덤블링 시전 시간
    public float tumblingTime;

    //디펜드 브레이크 애니메이션 클립
    public AnimationClip gaurdBreake;
    //공격 패턴 입력값 컨트롤 제어 변수들
    public bool isAtteck;
    public float inputTime;
    public float atkAnimTime;
    public int indexCount;
    //걷기 애니메이션 타이밍 제어용 플롯
    public float moveLength;
    //방어모드 제어 변수들
    public bool IsGaurd;
    public float speedDownPercentage;
    public float gaurdingTime;
    public float gaurdingLimite;
    //Defeat 모션 제어 변수
    public int defeatCount;

    //public TouchPad touchPadMove; //표준애셋 터치패드 대응용 : 보류중. 차후 삭제 가능

    public bool isActing;
    public bool startInputedCount;

    // Use this for initialization
    void Start()
    {
        playerBase = GetComponent<PlayerBaseScript>();
        tempAtteckCount = 0;
    }

    // 애니메이션 중 어택 카운트 제어하는 함수
    public void ComputeToAtteckCount()
    {
        //애니메이터의 인텍스 인티져를 확인, 그 인티져 값에 따라 진행
        
        if(animatorSelf.GetInteger("AtteckIndex") == 4) //애니메이터의 공격 인댁스 끝(4)와 같다면
        {
            tempAtteckCount = 4; //공격이 끝났을때 다음 공격으로 가는지 혹은 끝나는지를 비교하기 위함 임시 인티져
            if(atkAnimTime > 0)
            {
                indexCount = 0; //인풋시간이 남아있을때 인댁스는 0의로 변경
            }
        }
        else
        {
            //애니메이터의 공격 인택스가 끝이 아니라면 그 값을 받아와 임시 인티져에 대입
            if (animatorSelf.GetInteger("AtteckIndex") != 4) tempAtteckCount = animatorSelf.GetInteger("AtteckIndex");
            
            if (atkAnimTime > 0)  // 애니메이터의 공격 인댁스가 끝이 아니면
            {
                if (indexCount < 4) indexCount++; //끝번 인댁스보다 작으면 ++
                else indexCount = 4;    // 끝번과 같다면 고정
            }
        }
    }
    //어택 버튼용 함수
    public void ClickToAtteckButton()
    {
        if(!animatorSelf.GetBool("Atteck")) animatorSelf.SetBool("Atteck", true); //공격 애니메이션은 돌리고
        if(!isAtteck) isAtteck = true; //공격 불값 트루로 조정.

        if (startInputedCount) //인풋 시간이 시작되면
        {
            ComputeToAtteckCount(); //어택 인댁스 계산
        }
    }
    //어택 애니메이션 플레이때 돌아가는 타이머
    public void AtkAnimTimer()
    {
        if (isAtteck) // 공격중이라면
        {
            if (atkAnimTime > 0)
            {
                atkAnimTime -= Time.deltaTime; // 공격 애니메이션 시간이 남아 있다면 시간을 깎고
                startInputedCount = true; //인풋타임 시작
            }
            
            else 
            {
                //인풋타임이 끝났고 공격임시인티져와 인댁스인티져가 같지 않으면(인풋타임동안 버튼이 클릭되었다.)
                if (tempAtteckCount != indexCount)
                {
                    animatorSelf.SetInteger("AtteckIndex", indexCount); //인댁스를 변경.
                    ComputeAnimationPlayTime(); //애니메이션 시간 재정립.
                    startInputedCount = false;
                }
                //인풋타임이 끝났고 공격임시인티져와 인댁스인티져가 같으면(인풋타임동안 버튼이 클릭되지 않았다.)
                else
                {
                    animatorSelf.SetBool("Atteck", false); //공격 애니메이션을 중지하고
                    indexCount = 0;
                    animatorSelf.SetInteger("AtteckIndex", indexCount); //어택 인댁스는 초기화
                    ComputeAnimationPlayTime(); //어택 애니메이션 타임 재정립.
                    isAtteck = false; //공격 불값 펄스로 변경.
                    startInputedCount = false;
                    
                }
            }
        }
    }
    //애니메이션 시간 재정립 함수
    void ComputeAnimationPlayTime()
    {
        //애니메이션 시간 재정립하기 위해 애니메이터의 "Atteckindex" 값을 받아와 비교
        if (animatorSelf.GetInteger("AtteckIndex") == 0) // ex) "Atteckindex"가 0과 같다면
        {
            //어택애니메이션 클립의 0번 배열의 길이(플레이 타임)에 인풋타임 더한 값을 어택애니메타임에 대입.
            atkAnimTime = atkAnim[0].length + inputTime;
        }
        else if (animatorSelf.GetInteger("AtteckIndex") == 1)
        {
            atkAnimTime = atkAnim[1].length + inputTime;
        }
        else if (animatorSelf.GetInteger("AtteckIndex") == 2)
        {
            atkAnimTime = atkAnim[2].length + inputTime;
        }
        else if (animatorSelf.GetInteger("AtteckIndex") == 3)
        {
            atkAnimTime = atkAnim[3].length + inputTime;
        }
        else if (animatorSelf.GetInteger("AtteckIndex") == 4)
        {
            atkAnimTime = atkAnim[4].length + inputTime;
        }

        tempAtteckCount = animatorSelf.GetInteger("AtteckIndex");
    }
    // 후진 애니메이션 작동 함수
    public void PlayerBackMove()
    {
        animatorSelf.SetBool("SideStep", true);
    }
    // 전진 애니메이션 작동 함수
    public void PlayerFrontMove()
    {
        animatorSelf.SetBool("FrontStep", true);
    }
    // 이동 애니메이션 초기화 함수. 애니메이터 자체에서 재어하려 했으나 이동 중간에 대미지를 입거나 하는 변수를 대응하기위해 함수화 함.
    public void ReleaseToMoveAnimation()
    {
        if (animatorSelf.GetBool("SideStep")) animatorSelf.SetBool("SideStep", false);
        if (animatorSelf.GetBool("FrontStep")) animatorSelf.SetBool("FrontStep", false);
    }
    // 이동 스피드 계산 함수
    public float ComputeWalkSpeed()
    {
        float temp = 0;

        if (!animatorSelf.GetBool("Gaurd")) //방어태세가 아니라면
        {
            if (animatorSelf.GetBool("FrontStep")) // 진진이면
            {
                temp = moveLength / walkAnim[0].length;
            }
            else if (animatorSelf.GetBool("SideStep")) // 후진이면
            {
                temp = moveLength / walkAnim[1].length;
            }
            else temp = 0;
        }
        else // 방어태세라면
        {
            if (animatorSelf.GetBool("FrontStep")) //전진이면
            {
                temp = (moveLength / walkAnim[2].length) * speedDownPercentage;
                // speedDownPercentagesms 1과 가까워 질수록 정상 속도
            }
            else if (animatorSelf.GetBool("SideStep")) // 후진이면
            {
                temp = (moveLength / walkAnim[3].length) * speedDownPercentage;
            }
            else temp = 0;
        }
        return temp;
    }
    //방어태세 버튼 제어 함수
    public void ClickToGaurdButton()
    {
        if(!animatorSelf.GetBool("Gaurd"))
        {
            animatorSelf.SetBool("Gaurd", true);
            IsGaurd = true;
        }
    }
    //방어태세 유지 시간 타이머
    public void CheckToGaurdingTime()
    {
        if(IsGaurd)
        {
            if(gaurdingTime < gaurdingLimite) gaurdingTime += Time.deltaTime;
            else
            {
                IsGaurd = false;
                gaurdingTime = 0;
                animatorSelf.SetBool("Gaurd", false);
            }
        }
    }
    //패배 모션(죽음 모션) 랜덤재생 제어 함수
    public void RandomDefeat()
    {
        if(animatorSelf.GetBool("Defeat"))
        {
            defeatCount = Random.Range(1, 4);
            animatorSelf.SetInteger("DefeatCount", defeatCount);
        }
    }
    //데미지 애니메이션 재생
    public void PlayToGetDamageAnimation()
    {
        animatorSelf.SetBool("Damage", true);
    }
    //큰데미지 애니메이션 재생
    public void PlayToGethugeDamageAnimation()
    {
        animatorSelf.SetBool("HugeDamage", true);
    }
    //구르기 속도 함수
    public void ComputeToTumblingSpeed()
    {
        if(!animatorSelf.GetBool("Switch")) //기본 자세이면
        {
            if (animatorSelf.GetBool("Tumbling")) // 앞구르기 재생시간
            {
                tumblingSpeed = tumblingLength / tumbling[0].length;
            }
            else if (animatorSelf.GetBool("BackTumbling")) //뒤구르기 재생시간
            {
                tumblingSpeed = tumblingLength / backTumbling[0].length;
            }

        }
        else //활 자세라면
        {
            if (animatorSelf.GetBool("Tumbling")) // 앞구르기 재생시간
            {
                tumblingSpeed = tumblingLength / tumbling[1].length;
            }
            else if (animatorSelf.GetBool("BackTumbling")) // 뒤구르기 재생시간
            {
                tumblingSpeed = tumblingLength / backTumbling[1].length;
            }
        }
    }
    //앞구르기 재어 함수
    public IEnumerator PlayToTumbling()
    {
        if (animatorSelf.GetBool("Switch"))
        {
            if (animatorSelf.GetBool("Tumbling"))

            {
                if (tumblingTime < tumbling[0].length)
                {
                    tumblingTime += Time.deltaTime;
                    animatorSelf.gameObject.transform.Translate(tumblingSpeed, 0, 0);
                }
                else
                {
                    tumblingTime = 0;
                    tumblingSpeed = 0;
                    animatorSelf.SetBool("Timbling", false);
                }
            }
        }
        else
        {
            if (animatorSelf.GetBool("Tumbling"))

            {
                if (tumblingTime < tumbling[1].length)
                {
                    tumblingTime += Time.deltaTime;
                    animatorSelf.gameObject.transform.Translate(tumblingSpeed, 0, 0);
                }
                else
                {
                    tumblingTime = 0;
                    tumblingSpeed = 0;
                    animatorSelf.SetBool("Timbling", false);
                }
            }
        }
        yield return null;
    }
    //뒤구르기 재어함수
    public IEnumerator PlayToBackTumbling()
    {
        if(!animatorSelf.GetBool("Switch"))
        {
            if (animatorSelf.GetBool("BackTumbling"))
            {
                if (tumblingTime < backTumbling[0].length)
                {
                    tumblingTime += Time.deltaTime;
                    animatorSelf.gameObject.transform.Translate(-tumblingSpeed, 0, 0);
                }
                else
                {
                    tumblingTime = 0;
                    tumblingSpeed = 0;
                    animatorSelf.SetBool("BackTumbling", false);
                }
            }
        }
        else
        {
            if (animatorSelf.GetBool("BackTumbling"))
            {
                if (tumblingTime < backTumbling[1].length)
                {
                    tumblingTime += Time.deltaTime;
                    animatorSelf.gameObject.transform.Translate(-tumblingSpeed, 0, 0);
                }
                else
                {
                    tumblingTime = 0;
                    tumblingSpeed = 0;
                    animatorSelf.SetBool("BackTumbling", false);
                }
            }
        }
        yield return null;
    }
}
