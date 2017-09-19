using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundControll : MonoBehaviour
{
    public GameObject[] cloud;
    public GameObject[] land5;
    public GameObject[] land4;
    public GameObject[] fog;
    public GameObject[] land3;
    public GameObject[] land2;
    public GameObject[] land1;
    public GameObject[] landObject1;
    public GameObject[] landObject2;
    public GameObject[] landCollider;

    public Vector2 rightGoal;
    public Vector2 leftGoal;

    public float speed;

    public IEnumerator MoveToLeftAllBackGround()
    {
        for (int i = 0; i < 2; ++i)
        {
            if (land5[i].transform.position.x >= 20.43f)
            {
                land5[i].transform.position = new Vector2(-20.43f, land5[i].transform.position.y);
                land5[i].transform.Translate((speed * 0.05f), 0, 0);
            }
            else land5[i].transform.Translate((speed * 0.05f), 0, 0);

            if (land4[i].transform.position.x >= 20.43f)
            {
                land4[i].transform.position = new Vector2(-20.43f, land4[i].transform.position.y);
                land4[i].transform.Translate((speed * 0.07f), 0, 0);
            }
            else land4[i].transform.Translate((speed * 0.07f), 0, 0);

            if (land3[i].transform.position.x >= 20.43f)
            {
                land3[i].transform.position = new Vector2(-20.43f, land3[i].transform.position.y);
                land3[i].transform.Translate((speed * 0.3f), 0, 0);
            }
            else land3[i].transform.Translate((speed * 0.3f), 0, 0);

            if (land2[i].transform.position.x >= 20.43f)
            {
                land2[i].transform.position = new Vector2(-20.43f, land2[i].transform.position.y);
                land2[i].transform.Translate((speed * 0.7f), 0, 0);
            }
            else land2[i].transform.Translate((speed * 0.7f), 0, 0);

            if (land1[i].transform.position.x >= 20.43f)
            {
                land1[i].transform.position = new Vector2(-20.43f, land1[i].transform.position.y);
                land1[i].transform.Translate((speed * 0.7f), 0, 0);
            }
            else land1[i].transform.Translate((speed * 0.7f), 0, 0);

            if (landCollider[i].transform.position.x >= 20.43f)
            {
                landCollider[i].transform.position = new Vector2(-20.43f, landCollider[i].transform.position.y);
                landCollider[i].transform.Translate((speed * 0.7f), 0, 0);
            }
            else landCollider[i].transform.Translate((speed * 0.7f), 0, 0);

            if (landObject1[i].transform.position.x >= 20.43f)
            {
                landObject1[i].transform.position = new Vector2(-20.43f, landObject1[i].transform.position.y);
                landObject1[i].transform.Translate((speed*0.8f), 0, 0);
            }
            else landObject1[i].transform.Translate((speed*0.8f), 0, 0);

            if (landObject2[i].transform.position.x >= 20.43f)
            {
                landObject2[i].transform.position = new Vector2(-20.43f, landObject2[i].transform.position.y);
                landObject2[i].transform.Translate(speed, 0, 0);
            }
            else landObject2[i].transform.Translate(speed, 0, 0);

        }

        yield return null;
    }

    public IEnumerator MovetoRightAllBackGround()
    {
        for (int i = 0; i < 2; ++i)
        {
            if (land5[i].transform.position.x <= -20.43f)
            {
                land5[i].transform.position = new Vector2(20.43f, land5[i].transform.position.y);
                land5[i].transform.Translate((-speed * 0.05f), 0, 0);
            }
            else land5[i].transform.Translate((-speed * 0.05f), 0, 0);

            if (land4[i].transform.position.x <= -20.43f)
            {
                land4[i].transform.position = new Vector2(20.43f, land4[i].transform.position.y);
                land4[i].transform.Translate((-speed * 0.07f), 0, 0);
            }
            else land4[i].transform.Translate((-speed * 0.07f), 0, 0);

            if (land3[i].transform.position.x <= -20.43f)
            {
                land3[i].transform.position = new Vector2(20.43f, land3[i].transform.position.y);
                land3[i].transform.Translate((-speed * 0.3f), 0, 0);
            }
            else land3[i].transform.Translate((-speed * 0.3f), 0, 0);

            if (land2[i].transform.position.x <= -20.43f)
            {
                land2[i].transform.position = new Vector2(20.43f, land2[i].transform.position.y);
                land2[i].transform.Translate((-speed * 0.7f), 0, 0);
            }
            else land2[i].transform.Translate((-speed * 0.7f), 0, 0);

            if (land1[i].transform.position.x <= -20.43f)
            {
                land1[i].transform.position = new Vector2(20.43f, land1[i].transform.position.y);
                land1[i].transform.Translate((-speed * 0.7f), 0, 0);
            }
            else land1[i].transform.Translate((-speed * 0.7f), 0, 0);

            if (landCollider[i].transform.position.x <= -20.43f)
            {
                landCollider[i].transform.position = new Vector2(20.43f, landCollider[i].transform.position.y);
                landCollider[i].transform.Translate((-speed * 0.7f), 0, 0);
            }
            else landCollider[i].transform.Translate((-speed * 0.7f), 0, 0);

            if (landObject1[i].transform.position.x <= -20.43f)
            {
                landObject1[i].transform.position = new Vector2(20.43f, landObject1[i].transform.position.y);
                landObject1[i].transform.Translate((-speed * 0.8f), 0, 0);
            }
            else landObject1[i].transform.Translate((-speed * 0.8f), 0, 0);

            if (landObject2[i].transform.position.x <= -20.43f)
            {
                landObject2[i].transform.position = new Vector2(20.43f, landObject2[i].transform.position.y);
                landObject2[i].transform.Translate(-speed, 0, 0);
            }
            else landObject2[i].transform.Translate(-speed, 0, 0);

        }
        yield return null;
    }

    public IEnumerator MoveToCloudAndBlizzard()
    {
        for(int i =0; i < 2; ++i)
        {
            if (cloud[i].transform.position.x <= -20.43f)
            {
                cloud[i].transform.position = new Vector2(20.43f, cloud[i].transform.position.y);
                cloud[i].transform.Translate(-0.005f, 0, 0);
            }
            else cloud[i].transform.Translate(-0.005f, 0, 0);

            if (fog[i].transform.position.x <= -20.43f)
            {
                fog[i].transform.position = new Vector2(20.43f, fog[i].transform.position.y);
                fog[i].transform.Translate(-0.004f, 0, 0);
            }
            else fog[i].transform.Translate(-0.004f, 0, 0);
        }

        yield return null;
    }
}
