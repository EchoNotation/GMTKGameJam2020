using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed = 2f;
    public GameObject explosion;
    public AudioSource[] sources;
    private System.Diagnostics.Stopwatch timer;
    private long expireTime = 4000;
    private bool hitSomething = false;
    private int sourceNum;

    // Start is called before the first frame update
    void Start()
    {
        timer = new System.Diagnostics.Stopwatch();
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.ElapsedMilliseconds > expireTime) Destroy(this.gameObject);

        Vector3 current = this.transform.position;
        Vector3 velocityToAdd = direction.normalized * speed * Time.deltaTime;
        this.transform.position = new Vector3(current.x + velocityToAdd.x, current.y + velocityToAdd.y, 0);

        if (hitSomething && !sources[sourceNum].isPlaying) Destroy(this.gameObject);
    }
    public void setTrajectory(Vector3 direction)
    {
        this.direction = direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    public void addSpeed(float speedToAdd)
    {
        speed += speedToAdd;
    }

    public void onHit(bool hitRock)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        hitSomething = true;
        transform.GetComponent<SpriteRenderer>().enabled = false;
        transform.GetComponent<BoxCollider2D>().enabled = false;

        if (hitRock)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                sourceNum = 0;
            }
            else
            {
                sourceNum = 1;
            }
        }
        else
        {
            sourceNum = 2;
        }

        sources[sourceNum].Play();
    }
}
