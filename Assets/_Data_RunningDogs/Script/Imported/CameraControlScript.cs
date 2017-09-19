using UnityEngine;
using System.Collections;

public class CameraControlScript : MonoBehaviour {

    public PlayerBaseScript player;
    public StageControlScript stage;

    float distanceToMove;
    float offsetWithPlayer;
    float currentPosition;

    private void Start()
    {
        distanceToMove = 2.0f;
        offsetWithPlayer = 3.0f;
    }

    // Update is called once per frame
    void Update () {

        if (stage.controlStatus == StageControlScript.ControlStatus.MainGame_Enabled)
        {
            currentPosition = transform.position.x;
            float currentDistance = currentPosition - player.transform.position.x - offsetWithPlayer;

            if (currentDistance < -distanceToMove)
            {
                transform.position = new Vector3(-distanceToMove + player.transform.position.x + offsetWithPlayer,
                    transform.position.y,
                    transform.position.z);
            }

            if (currentDistance > distanceToMove)
            {
                transform.position = new Vector3(distanceToMove + player.transform.position.x + offsetWithPlayer,
                    transform.position.y,
                    transform.position.z);
            }
        }
    }
}
