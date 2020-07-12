using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float endRadius = 3f;
    public float startRadius = 8f;
    public float fallTime = 4f;

    public float killRadius = 2f;

    public GameObject explosion;
    public AudioSource[] sources;

    private bool hasExploded;

    float currentTime = 0f;

    GameObject indicator;

    private void Start()
    {
        indicator = transform.GetChild(0).gameObject;
        sources[0].Play();
    }

    private void FixedUpdate()
    {
        indicator.transform.localScale = Vector3.one * Mathf.Lerp(startRadius, endRadius, currentTime / fallTime);

        currentTime += Time.deltaTime;

        if(currentTime > fallTime && !hasExploded)
        {
            Explode();
        }

        if (hasExploded & !sources[1].isPlaying) Destroy(this.gameObject);
    }

    void Explode()
    {
        //boom
        hasExploded = true;

        Instantiate(explosion, transform.position, Quaternion.identity);
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;

        //kill stuff in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), killRadius);
        foreach(Collider2D col in hits)
        {
            if(col.CompareTag("Enemy"))
            {
                col.gameObject.GetComponent<Enemy>().die();
            }
            else if(col.CompareTag("Player"))
            {
                col.gameObject.GetComponent<Player>().die();
            }
        }

        sources[0].Stop();
        sources[1].Play();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, killRadius);
    //}
}
