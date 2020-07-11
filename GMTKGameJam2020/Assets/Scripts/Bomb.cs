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
    public AudioSource source;

    private bool hasExploded;

    float currentTime = 0f;

    GameObject indicator;

    private void Start()
    {
        indicator = transform.GetChild(0).gameObject;
    }

    private void FixedUpdate()
    {
        indicator.transform.localScale = Vector3.one * Mathf.Lerp(startRadius, endRadius, currentTime / fallTime);

        currentTime += Time.deltaTime;

        if(currentTime > fallTime && !hasExploded)
        {
            Explode();
        }

        if (hasExploded & !source.isPlaying) Destroy(this.gameObject);
    }

    void Explode()
    {
        //boom
        hasExploded = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;
        float distSquared = Mathf.Pow((transform.position.x - playerPos.x), 2f) + Mathf.Pow((transform.position.y - playerPos.y), 2f);
        if(killRadius > distSquared)
        {
            Debug.Log("Bomb killed player from " + distSquared);
            player.GetComponent<Player>().die();
        }
        Instantiate(explosion, transform.position, Quaternion.identity);
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        source.Play();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, killRadius);
    //}
}
