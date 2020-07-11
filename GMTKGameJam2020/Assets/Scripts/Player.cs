using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Control Types
    private bool isShootMode = false;
    public float timeToSwap;
    private static int SWAP_DURATION = 6;

    //Control Info
    private Vector3 shootDirection;
    private Vector3 moveDirection;
    public float speed = .1f;
    public static int ATTACK_DELAY = 20;
    private int atkTimer = 0;
    public GameObject Bullet;

    //Player Input
    private Vector3 screenCenter;

    // 0 means no powerup active
    private int activePowerup = 0;
    // time left on the powerup timer
    [SerializeField]
    private float powerUpTimeout = 0;

    // Start is called before the first frame update
    void Start()
    {
        timeToSwap = SWAP_DURATION;
        screenCenter = new Vector3(Screen.width/2, Screen.height/2, 0);
        shootDirection = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Manage Timing of Swapping Control Types
        timeToSwap -= Time.deltaTime;
        if (timeToSwap <= 0)
        {
            timeToSwap = SWAP_DURATION;
            if (isShootMode){
                isShootMode = false;
            }
            else{
                isShootMode = true;
            }
        }
        
        //Player Input
        Vector3 direction = Input.mousePosition - screenCenter;
        OnInput(direction);

        //Automatic Controls
        this.gameObject.transform.Translate(moveDirection * speed * Time.deltaTime);
        if (atkTimer <= 0)
        {
            atkTimer = ATTACK_DELAY;
            GameObject shot = Instantiate(Bullet, transform.position, Quaternion.identity);
            shot.GetComponent<Bullet>().setTrajectory(shootDirection);
        }
        else
        {
            atkTimer -= 1;
        }


        if(powerUpTimeout > 0)
        {
            //decrease active powerup
            powerUpTimeout -= Time.deltaTime;
        }
        else if(activePowerup != 0 && powerUpTimeout <= 0)
        {
            DeactivatePowerup();
            powerUpTimeout = 0;
        }

    }

    private void OnInput(Vector2 direction)
    {
        if (isShootMode)
        {
            shootDirection = direction.normalized;
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            this.gameObject.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
        else
        {
            moveDirection = direction.normalized;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            this.gameObject.transform.GetChild(1).rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }
    }


    private void die()
    {
        Debug.Log("Player died!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "EnemyBullet":
                Destroy(collision.gameObject);
                die();
                break;
            case "Powerup":
            case "Bullet":
                break;
            default:
                Debug.Log("Unrecognized tag in OnTriggerEnter2D in Player! Tag: " + collision.tag);
                break;
        }
    }


    //yes I know I am very lazy with this code, but hey game jam - CG
    //this function is activated by a powerup
    public void ActivatePowerup(int powerup, float time)
    {
        //prevent powerup stacking, powerup = 0 means no powerup active
        if(activePowerup != 0)
        {
            DeactivatePowerup();
        }

        activePowerup = powerup;
        switch(powerup)
        {
            //0 = no powerup, not sure how it would be called here
            case 0:
                Debug.LogWarning("[Player]: Logic error: somehow activated no powerup");
                break;
            
            //1 = more speed
            case 1:
                speed *= 2;
                break;

            default:
                Debug.LogWarning("[Player]: Warning: Logic Error: powerup ID not recongized");
                break;
        }

        powerUpTimeout = time;
    }

    private void DeactivatePowerup()
    {
        switch(activePowerup)
        {
            case 0:
                break;

            case 1:
                speed /= 2;
                break;

            default:
                //Debug.LogWarning("Deactivating unrecognized powerup");
                break;
        }
        //Debug.Log("powerup expired");
        activePowerup = 0;
        powerUpTimeout = 0;
    }


}

