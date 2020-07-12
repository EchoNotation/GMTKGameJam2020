using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed = 2f;
    public GameObject explosion;
    public AudioSource[] sources;
    private System.Diagnostics.Stopwatch timer;
    private long expireTime = 4000;
    private bool hitSomething = false;
    private float killRadius = 0.47f;

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

        if (hitSomething && !sources[0].isPlaying && !sources[1].isPlaying) Destroy(this.gameObject);
    }
    public void setTrajectory(Vector3 direction)
    {
        this.direction = direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    public void onHit()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        hitSomething = true;
        transform.GetComponent<SpriteRenderer>().enabled = false;
        transform.GetComponent<BoxCollider2D>().enabled = false;

        sources[0].Play();

        Collider2D[] hits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), killRadius);
        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Enemy"))
            {
                col.gameObject.GetComponent<Enemy>().die();
                sources[1].Play();
            }
            else if (col.CompareTag("Player"))
            {
                col.gameObject.GetComponent<Player>().die();
            }
        }
    }
}
