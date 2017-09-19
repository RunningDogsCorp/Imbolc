using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSystem : MonoBehaviour
{
    public SpriteRenderer logo;
    public float speed;
    public float alpha;
    public float time;
    public bool reverce;

	void Update ()
    {
        if(!reverce)
        {
            if (alpha <= 1.0f)
            {
                alpha += speed;
            }
            else if (alpha > 1.0f)
            {
                reverce = true;
            }
        }
        else
        {
            if (time >= 0) time -= Time.deltaTime;
            else
            {
                if (alpha >= 0) alpha -= speed;
                else
                {
                    SceneManager.LoadScene(3);
                }
            }
        }

        logo.color = new Color(1.0f, 1.0f, 1.0f, alpha);
	}
}
