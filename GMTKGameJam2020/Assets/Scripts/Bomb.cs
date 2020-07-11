using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float endRadius = 3f;
    public float startRadius = 8f;
    public float maxTime = 5f;
    public float minTime = 3f;

    float startTime;

    private float explodeTime;

    private void Start()
    {
        startTime = Time.fixedTime;
        explodeTime = Random.Range(minTime, maxTime);
    }

    private void FixedUpdate()
    {
        //transform.localScale *= Mathf.Lerp(endRadius, startRadius, transform.localScale.magnitude / time);
        if(Time.fixedTime - startTime > explodeTime)
        {
            Explode();
        }
    }

    void Explode()
    {
        //boom

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;
        float distSquared = Mathf.Pow((transform.position.x - playerPos.x), 2f) + Mathf.Pow((transform.position.y - playerPos.y), 2f);
        if(endRadius * endRadius > distSquared)
        {
            Debug.Log("Bomb killed player from " + distSquared);
            player.GetComponent<Player>().die();
        }
        Destroy(gameObject);
    }
}
