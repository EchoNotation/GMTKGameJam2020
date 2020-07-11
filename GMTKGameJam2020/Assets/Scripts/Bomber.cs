using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public GameObject bomb;

    public float speed = 1f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 4f;
    public float minXoffset = -1f;
    public float maxXoffset = 1f;
    public float lifetime = 35f;

    public Vector3 offset = Vector3.zero;

    public void Start()
    {
        StartCoroutine("DoBombingRun");
        Destroy(gameObject, lifetime);
    }

    IEnumerator DoBombingRun()
    {
        //Debug.Log("Starting bombing run...");
        while(true)
        {
            SpawnBomb(Random.Range(minXoffset, maxXoffset));
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
        }
    }

    public void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);
    }

    public void SpawnBomb(float radius)
    {
        Vector3 location = transform.position + offset + new Vector3(Random.Range(minXoffset, maxXoffset), 0, 0);
        Instantiate(bomb, location, Quaternion.Euler(0,0,Random.Range(0,360)));
    }

    public void OnDestroy()
    {
        StopAllCoroutines();
    }

}
