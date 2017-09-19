using UnityEngine;
using System.Collections;

public class StageControlScript : MonoBehaviour {

    public enum ControlStatus
    {
        Disabled,
        MainGame_Enabled,
        Menu_UI_Enabled,
    }

    public ControlStatus controlStatus;
    //public GameObject mainGameController;

    //스테이지 완료 감지용. 시나리오 클래스에서 이 값을 받아가서 시나리오를 완료해야 한다.
    public bool readyForComplete;
    
	// Use this for initialization
	void Start () {

        //SetControlStatus(ControlStatus.Disabled);
        //readyForComplete = false;

        SetControlStatus(ControlStatus.MainGame_Enabled);
        readyForComplete = true;
	}

    public void SetControlStatus(ControlStatus status)
    {
        controlStatus = status;

        //if (controlStatus == ControlStatus.MainGame_Enabled)
        //{
        //    mainGameController.SetActive(true);
        //}
        //else if (controlStatus == ControlStatus.Menu_UI_Enabled)
        //{
        //    mainGameController.SetActive(false);
        //}
        //else if (controlStatus == ControlStatus.Disabled)
        //{
        //    mainGameController.SetActive(false);
        //}

    }
}
