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
    private int dodgeCounter;

    private int strafeDirection;

    private bool dodgingPit;
    private bool dodgingLeft;
    private Vector3 pitDir;
    private Vector3 currentDir;

    public Sprite[] bodySprites, toolSprites;
    public GameObject bullet;

    private bool sawSwitch = true;

    private float chargerSpeed = 2f;
    private float gunnerSpeed = 2f;
    private float speed = 0.5f;

    private float gunnerMinDist = 0.8f;
    private float gunnerMaxDist = 1.4f;

    // Start is called before the first frame update
    void Start()
    {
        counterReq = 3;
        logicCounter = 0;
        shotCounter = 0;
        shotReq = 30;
        dodgingPit = false;

        if(Random.Range(0, 2) == 1) strafeDirection = 1;

        updateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if(dodgingPit)
        {
            dodgePit();
        }

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

    void dodgePit()
    {
        Vector3 myPos = this.transform.position;
        Vector3 perpendicular = Vector3.Cross(pitDir, Vector3.forward);
        Vector3 velocityToAdd = perpendicular.normalized * speed * Time.deltaTime;

        if (dodgingLeft) velocityToAdd = -velocityToAdd;

        this.transform.position = new Vector3(myPos.x + velocityToAdd.x, myPos.y + velocityToAdd.y, 0);

        dodgeCounter--;

        if (dodgeCounter <= 0)
        {
            dodgingPit = false;
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
        rotateSaws(directionToPlayer);

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
            //Debug.Log(hit.collider.tag);

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
                this.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = toolSprites[0];
                break;
            case Enemies.GUNNER:
                this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = bodySprites[1];
                this.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = toolSprites[2];
                break;
            default:
                Debug.Log("Invalid enemyType in updateSprite! enemyType: " + enemyType);
                break;
        }
    }

    void rotateSaws(Vector3 direction)
    {
        Sprite temp;

        if (sawSwitch)
        {
            temp = toolSprites[0];
            sawSwitch = false;
        }
        else
        {
            temp = toolSprites[1];
            sawSwitch = true;
        }

        this.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = temp;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void OnDestroy()
    {
        //Make sound or play particle effect or something.   
    }

    private void rotateBody(Vector3 direction)
    {
        if(dodgingPit) return;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        this.gameObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        currentDir = direction;
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
            case "Pit":
                dodgingPit = true;

                ContactPoint2D[] contacts = new ContactPoint2D[1];
                collision.GetContacts(contacts);
                Vector3 toCollision = new Vector3(contacts[0].point.x, contacts[0].point.y, 0) - this.transform.position;
                Vector3 toPitCenter = collision.gameObject.transform.position - this.transform.position;
                float angle = Vector3.SignedAngle(toPitCenter, toCollision, Vector3.forward);
                if (angle < 0) dodgingLeft = true;
                else dodgingLeft = false;             

                dodgeCounter = 20;
                pitDir = currentDir;
                break;
            default:
                Debug.Log("Unrecognized tag in OnTriggerEnter2D in Enemy! Tag: " + collision.tag);
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Pit"))
        {
            if(!dodgingPit)
            {
                dodgingPit = true;
                pitDir = currentDir;

                ContactPoint2D[] contacts = new ContactPoint2D[1];
                collision.GetContacts(contacts);
                Vector3 toCollision = new Vector3(contacts[0].point.x, contacts[0].point.y, 0) - this.transform.position;
                Vector3 toPitCenter = collision.gameObject.transform.position - this.transform.position;
                float angle = Vector3.SignedAngle(toPitCenter, toCollision, Vector3.forward);

                if (angle < 0) dodgingLeft = true;
                else dodgingLeft = false;
            }

            dodgeCounter += 2;
        }
    }
}
