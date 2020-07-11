using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public enum Enemies
{
    CHARGER,
    GUNNER,
}

public class Enemy : MonoBehaviour
{
    public Enemies enemyType;
    private int logicCounter, counterReq;
    private int shotCounter, shotReq;

    private int strafeDirection;

    //0: Charger, 1: Gunner
    public Sprite[] bodySprites, barrelSprites;
    public GameObject bullet;

    private float chargerSpeed = 3f;
    private float gunnerSpeed = 3f;

    private float gunnerMinDist = 1f;
    private float gunnerMaxDist = 1.4f;

    // Start is called before the first frame update
    void Start()
    {
        counterReq = 3;
        logicCounter = 0;
        shotCounter = 0;
        shotReq = 30;

        if(Random.Range(0, 2) == 1) strafeDirection = 1;

        updateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if(logicCounter >= counterReq)
        {
            logicCounter = 0;

            switch(enemyType)
            {
                case Enemies.CHARGER:
                    chargerLogic();
                    break;
                case Enemies.GUNNER:
                    gunnerLogic();
                    break;
                default:
                    Debug.Log("Invalid enemyType encountered during logic update! enemyType: " + enemyType);
                    break;
            }
        }
        else
        {
            logicCounter++;
        }
    }

    void chargerLogic()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 myPos = this.transform.position;
        Vector3 directionToPlayer = new Vector3(playerPos.x - myPos.x, playerPos.y - myPos.y, 0);
        Vector3 velocityToAdd = directionToPlayer.normalized * chargerSpeed * Time.deltaTime;
        rotateBody(directionToPlayer);

        this.transform.position = new Vector3(myPos.x + velocityToAdd.x, myPos.y + velocityToAdd.y, 0);
    }

    void gunnerLogic()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player.transform.position;
        Vector3 myPos = this.transform.position;
        Vector3 directionToPlayer = new Vector3(playerPos.x - myPos.x, playerPos.y - myPos.y, 0);
        Vector3 velocityToAdd = new Vector3();
        rotateBarrel(directionToPlayer);
        shotCounter++;

        //Debug.Log(shotCounter);

        if(directionToPlayer.magnitude > gunnerMaxDist)
        {
            //Need to move into range!
            velocityToAdd = directionToPlayer.normalized * gunnerSpeed * Time.deltaTime;
            rotateBody(velocityToAdd);
            this.transform.position = new Vector3(myPos.x + velocityToAdd.x, myPos.y + velocityToAdd.y, 0);
        }
        else if(directionToPlayer.magnitude < gunnerMinDist)
        {
            //Need to get away!
            velocityToAdd = directionToPlayer.normalized * -gunnerSpeed * Time.deltaTime;
            rotateBody(velocityToAdd);
            this.transform.position = new Vector3(myPos.x + velocityToAdd.x, myPos.y + velocityToAdd.y, 0);
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(myPos, directionToPlayer);
            Debug.Log(hit.collider.tag);

            if(hit.collider.CompareTag("Player") && shotCounter >= shotReq)
            {
                //Have a clear shot... fire!
                //Debug.Log("Firing!");
                shotCounter = 0;
                GameObject projectile = Instantiate(bullet, this.transform.position, Quaternion.identity);
                projectile.GetComponent<EnemyBullet>().setTrajectory(directionToPlayer);
            }
            else
            {
                //Strafe around the player.
                velocityToAdd = Vector3.Cross(directionToPlayer, new Vector3(0, 0, 1));
                velocityToAdd = velocityToAdd.normalized * gunnerSpeed * Time.deltaTime;

                if(strafeDirection == 0)
                {
                    velocityToAdd = -velocityToAdd;
                }

                rotateBody(velocityToAdd);
                this.transform.position = new Vector3(myPos.x + velocityToAdd.x, myPos.y + velocityToAdd.y, 0);
            }
        }
    }

    void updateSprite()
    {
        switch(enemyType)
        {
            case Enemies.CHARGER:
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = bodySprites[0];
                break;
            case Enemies.GUNNER:
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = bodySprites[1];
                this.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = barrelSprites[1];
                break;
            default:
                Debug.Log("Invalid enemyType in updateSprite! enemyType: " + enemyType);
                break;
        }
    }

    private void OnDestroy()
    {
        //Make sound or play particle effect or something.   
    }

    private void rotateBody(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.gameObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void rotateBarrel(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Bullet":
                Destroy(this.gameObject);
                Destroy(collision.gameObject);
                break;
            case "EnemyBullet":
            case "Powerup":
                break;
            default:
                Debug.Log("Unrecognized tag in OnTriggerEnter2D in Enemy! Tag: " + collision.tag);
                break;
        }
    }

}
