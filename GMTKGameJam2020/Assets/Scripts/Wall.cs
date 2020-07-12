using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Bullet":
                collision.gameObject.GetComponent<Bullet>().onHit(true);
                break;
            case "PiercingBullet":
                collision.gameObject.GetComponent<PiercingBullet>().onHit(true);
                break;
            case "EnemyBullet":
                collision.gameObject.GetComponent<EnemyBullet>().onHit(true);
                break;
            default:
                break;
        }
    }
}
