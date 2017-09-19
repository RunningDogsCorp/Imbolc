using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderToPlayer : MonoBehaviour
{
    public StageControlScript stage;
    public bool playerCollision;
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(stage.controlStatus != StageControlScript.ControlStatus.Disabled)
            {
                playerCollision = true;

                /*if (this.gameObject.name == "LeftCamEndPos")
                {
                    playerCollision = true;
                }
                else if (this.gameObject.name == "RightCamEndPos")
                {
                    playerCollision = true;
                }*/
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        playerCollision = false;
    }
}
