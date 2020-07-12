using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed = 2f;
    public GameObject explosion;
    public AudioSource[] sources;
    public AudioSource[] hitSources;
    private System.Diagnostics.Stopwatch timer;
    private long expireTime = 4000;
    private bool hitWall = false;
    private int sourceNum;
    private int numberOfHits = 3;

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

        if ((hitWall && !sources[sourceNum].isPlaying) || (numberOfHits <= 0 && !hitSources[0].isPlaying)) Destroy(this.gameObject);
    }

    public void setTrajectory(Vector3 direction)
    {
        this.direction = direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    public void onHit(bool hitRock)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        numberOfHits--;

        if(hitRock || numberOfHits <= 0)
        {
            transform.GetComponent<SpriteRenderer>().enabled = false;
            transform.GetComponent<BoxCollider2D>().enabled = false;
        }
        
        if (hitRock)
        {
            hitWall = true;

            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                sourceNum = 0;
            }
            else
            {
                sourceNum = 1;
            }

            sources[sourceNum].Play();
        }
        else
        {
            hitSources[numberOfHits].Play();
        }
    }
}
