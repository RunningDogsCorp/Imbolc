using UnityEngine;
using System.Collections;

public class PlayerHitBoxScript : MonoBehaviour {

    float timePassed;
    float timeEnd;
    
    public void Initiate(float timeLength, float size_x = 1, float size_y = 1, float size_z = 1)
    {
        timePassed = 0;
        timeEnd = timeLength;
        transform.localScale = new Vector3(size_x, size_y, size_z);

        Debug.Log("Hit Box Initiated.");
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit : to Enemy");

            Npc_Enemy_AI enemyAI = col.GetComponent<Npc_Enemy_AI>();

            if (!enemyAI.action_ignoreHit)
            {
                enemyAI.StartSetAIsequence();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= timeEnd) gameObject.SetActive(false);
    }
}
