using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public GameObject bomb;

    public float speed = 1f;
    public float minWaitTime = 0.5f;
    public float maxWaitTime = 1f;
    public float minXoffset = -4f;
    public float maxXoffset = 4f;
    public float lifetime = 40f;

    public int minBombs = 3;
    public int maxBombs = 5;

    public float bombDropRange = 5f;

    private int bombs;

    public Vector3 offset = Vector3.zero;

    private Vector3 target = Vector3.zero;

    public void Start()
    {
        StartCoroutine("DoBombingRun");
        Destroy(gameObject, lifetime);
        bombs = Random.Range(minBombs, maxBombs);
        SetTarget(GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
        Vector3 direction = (target - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    IEnumerator DoBombingRun()
    {
        while(true)
        {
            if (Vector3.Distance(target, transform.position) < bombDropRange)
            {
                for (int i = 0; i < bombs; i++)
                {
                    SpawnBomb(Random.Range(minXoffset, maxXoffset));
                    yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
                }

                yield return null;
            }
            else yield return new WaitForFixedUpdate();
        }
    }

    public void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
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
