using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float endRadius = 3f;
    public float startRadius = 8f;
    public float maxTime = 5f;
    public float minTime = 3f;

    public float killRadius = 2f;

    float currentTime = 0f;

    private float explodeTime;

    GameObject indicator;

    private void Start()
    {
        explodeTime = Random.Range(minTime, maxTime);
        indicator = transform.GetChild(0).gameObject;
    }

    private void FixedUpdate()
    {
        indicator.transform.localScale = Vector3.one * Mathf.Lerp(startRadius, endRadius, currentTime / explodeTime);

        currentTime += Time.deltaTime;

        if(currentTime > explodeTime)
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
        if(killRadius > distSquared)
        {
            Debug.Log("Bomb killed player from " + distSquared);
            player.GetComponent<Player>().die();
        }
        Destroy(gameObject);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, killRadius);
    //}
}
