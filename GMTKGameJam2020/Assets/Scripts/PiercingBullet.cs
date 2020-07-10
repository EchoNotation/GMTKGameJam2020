using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed = 0.2f;
    private int numberOfObjectsPierced, maxNumberOfPierces;

    // Start is called before the first frame update
    void Start()
    {
        numberOfObjectsPierced = 0;
        maxNumberOfPierces = 3;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 current = this.transform.position;
        Vector3 velocityToAdd = direction.normalized * speed;
        this.transform.position = new Vector3(current.x + velocityToAdd.x, current.y + velocityToAdd.y, 0);
    }
    public void setTrajectory(Vector3 direction)
    {
        this.direction = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case "Enemy":
            case "EnemyBullet":
                Destroy(collision.collider.gameObject);
                numberOfObjectsPierced++;

                if (numberOfObjectsPierced >= maxNumberOfPierces) Destroy(this.gameObject);
                break;
            case "Wall":
            case "Forcefield":
                Destroy(this.gameObject);
                break;
            case "Bullet":
            default:
                Debug.Log("Unrecognized tag in Bullet! Collider tag: " + collision.collider.tag);
                break;
        }
    }
}
