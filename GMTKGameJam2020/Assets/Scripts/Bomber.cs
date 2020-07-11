using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public GameObject bomb;

    public float speed = 1f;
    public float waitTime = 5f;
    public int maxBombs = 5;
    public int minBombs = 0;
    public float minRadius = 0f;
    public float maxRadius = 3f;
    public float lifetime = 35f;

    public void Start()
    {
        StartCoroutine("DoBombingRun");
        Destroy(gameObject, lifetime);
    }

    IEnumerator DoBombingRun()
    {
        Debug.Log("Starting bombing run...");
        while(true)
        {
            SpawnBombs(Random.Range(minBombs, maxBombs), Random.Range(minRadius, maxRadius));
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);
    }

    public void SpawnBombs(int count, float radius)
    {
        Debug.Log("Dropping " + count + " bombs");
        for(int i = 0; i <= count; i++)
        {
            Vector2 temp = Random.insideUnitCircle * radius;
            Vector3 location = new Vector3(temp.x, temp.y) + transform.position;
            Instantiate(bomb, location, Quaternion.identity);
        }
    }

}
