using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControllSystem : MonoBehaviour
{
    public float speed; //이동속도
    public GameObject player; //플레이어 객체
    public ColliderToPlayer[] camEndPoints;//플레이어가 카메라 밖으로 가는걸 막는 트리거 
    public BackGroundControll bgCon; //위 트리거 발동시 배경 움직임 컨트롤러
    public StageControlScript stage; //스테이지 관련 스크립트

    public PlayerActionScript playerAction; // 플레이어 액션 제어 스크립트
    public float walkTime; //이동 시간

	Vector3 movePos;
	float moveDir;


	void Update()
	{
		//방어태세 시 방어태세 유지시간 타이머
		playerAction.CheckToGaurdingTime();
		//백그라운드의 안개 및 구름 움직임 제어 함수
		StartCoroutine(bgCon.MoveToCloudAndBlizzard());

		//공격중이면 작동하는 공격 타이머
		if (playerAction.isAtteck) playerAction.AtkAnimTimer();

		//시나리오 연출이 끝나면
		if (stage.controlStatus != StageControlScript.ControlStatus.Disabled)
		{
			//플레이어 가두는 트리거 객체 온
			for (int i = 0; i < camEndPoints.Length; ++i)
			{
				camEndPoints[i].gameObject.SetActive(true);
			}
		}
		//시나리오 연출이 끝나고
		if (stage.controlStatus == StageControlScript.ControlStatus.MainGame_Enabled)
		{

			moveDir = UltimateJoystick.GetHorizontalAxis("Move");

			if (UltimateJoystick.GetTapCount("Action"))
			{
				playerAction.ClickToAtteckButton();
			}
			
			// 왼쪽키를 누르면(이부분을 컨트롤러 부착 시 수정하면 됨)
			if (Input.GetKey(KeyCode.LeftArrow)||(moveDir < 0))
            {
                // 왼쪽 끝 트리거랑 충돌하지 않았다면
                if (!camEndPoints[0].playerCollision)
                {
                    speed = playerAction.ComputeWalkSpeed(); // 이동값을 계산하고
					if (moveDir != 0)
					{
						speed = speed * moveDir * -1;
					}
					player.transform.Translate(-speed, 0, 0); // 그 이동값으로 움직이라

				}
                else // 충돌했으면
                {
                    bgCon.speed = playerAction.ComputeWalkSpeed(); // 이동값을 계산하여 백그라운드 스피드로 대입
                    speed = 0; // 플레이어 이동속도는 0

                    StartCoroutine(bgCon.MoveToLeftAllBackGround()); // 백그라운드 이동 코루틴 시작
                }

                playerAction.PlayerBackMove(); // 플레이이 후진 애니메이션 동작
            }
            //오른쪽 키를 누르면
            else if (Input.GetKey(KeyCode.RightArrow)||(moveDir > 0))
            {
                // 오른쪽 끝 트리거랑 충돌하지 않았다면
                if (!camEndPoints[1].playerCollision)
                {
                    speed = playerAction.ComputeWalkSpeed(); //이동값 계산하고
					if (moveDir != 0)
					{
						speed = speed * moveDir;
					}
					player.transform.Translate(speed, 0, 0); // 계산된 이동값으로 객체를 이동
                }
                else // 충돌했다면
                {
                    bgCon.speed = playerAction.ComputeWalkSpeed(); // 이동값을 계산하여 백그라운드 스피드로 대입
                    speed = 0; // 플레이어 이동속도는 0

                    StartCoroutine(bgCon.MovetoRightAllBackGround());  // 백그라운드 이동 코루틴 시작
                }

                playerAction.PlayerFrontMove(); // 플레이어 전진 애니메이션 동작
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)|| (moveDir == 0)) //키입력이 끝나면
            {
                playerAction.ReleaseToMoveAnimation(); //플에이어 애니메이션 릴리즈
            }
        }
	}
    
}
