using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 current = this.transform.position;
        Vector3 velocityToAdd = direction.normalized * speed * Time.deltaTime;
        this.transform.position = new Vector3(current.x + velocityToAdd.x, current.y + velocityToAdd.y, 0);
    }

    public void setTrajectory(Vector3 direction)
    {
        this.direction = direction;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case "Enemy":
            case "EnemyBullet":
                break;
            case "Wall":
            case "Forcefield":
                Destroy(this.gameObject);
                break;
            case "Bullet":
            case "Player":
                //Destroy(collision.collider.gameObject);
                Destroy(this.gameObject);
                break;
            default:
                Debug.Log("Unrecognized tag in Bullet! Collider tag: " + collision.collider.tag);
                break;
        }
    }
}
