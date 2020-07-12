using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Control Types
    private bool isShootMode = false;
    public float timeToSwap;
    private static int SWAP_DURATION = 6;

    //Control Info
    private Vector3 shootDirection;
    private Vector3 moveDirection;
    public float speed = .75f;
    public int ATTACK_DELAY = 20;
    private int atkTimer = 0;
    public GameObject Bullet;
    public GameObject piercingBullet;
    public bool alive = true;
    private float score = 0f;
    public int displayScore = 0;
    public Sprite destroyed;
    public GameObject explosion;

    public Sprite grayBody;
    public Sprite grayTurret;
    public Sprite greenBody;
    public Sprite greenTurret;

    public AudioSource source;

    //Player Input
    private Vector3 screenCenter;

    //spawn point for bullets
    public Transform endOfTurret;

    // 0 means no powerup active
    private int activePowerup = 0;
    // time left on the powerup timer
    //[SerializeField]
    private float powerUpTimeout = 0;
    private float currentPowerupDuration = 1;

    // Start is called before the first frame update
    void Start()
    {
        timeToSwap = SWAP_DURATION;
        screenCenter = new Vector3(Screen.width/2, Screen.height/2, 0);
        shootDirection = Vector3.up;
        isShootMode = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = grayTurret;
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = greenBody;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (alive)
        {
            //manage scoring
            score += 10*Time.deltaTime;
            displayScore = (int)score - ((int)score%10);
            transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = displayScore.ToString();
            transform.GetChild(3).GetChild(3).GetComponent<Scrollbar>().size = 1 - (powerUpTimeout /currentPowerupDuration);

            //Manage Timing of Swapping Control Types
            timeToSwap -= Time.deltaTime;
            transform.GetChild(3).GetChild(0).GetComponent<Scrollbar>().size = 1 - (timeToSwap / SWAP_DURATION);
            if (timeToSwap <= 0)
            {
                timeToSwap = SWAP_DURATION;
                if (isShootMode)
                {
                    isShootMode = false;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = grayTurret;
                    transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = greenBody;
                }
                else
                {
                    isShootMode = true;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = greenTurret;
                    transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = grayBody;
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

                if(activePowerup == 3)
                {
                    GameObject shot = Instantiate(piercingBullet, endOfTurret.position, endOfTurret.rotation);
                    shot.GetComponent<PiercingBullet>().setTrajectory(shootDirection);
                }
                else
                {
                    GameObject shot = Instantiate(Bullet, endOfTurret.position, endOfTurret.rotation);
                    shot.GetComponent<Bullet>().setTrajectory(shootDirection);
                }
                

                if (source.isPlaying) source.Stop();
                source.Play();
                //shot.GetComponent<Bullet>().addSpeed(dotProduct(moveDirection, shootDirection));
            }
            else
            {
                atkTimer -= 1;
            }


            if (powerUpTimeout > 0)
            {
                //decrease active powerup
                powerUpTimeout -= Time.deltaTime;
            }
            else if (activePowerup != 0 && powerUpTimeout <= 0)
            {
                DeactivatePowerup();
                powerUpTimeout = 0;
            }
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


    public void die()
    {
        if(!alive) return;
        Debug.Log("Player died!");
        alive = false;
        Instantiate(explosion, this.transform.position, Quaternion.identity);
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = destroyed;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = greenTurret;

        //prevent tank from moving after destroyed
        //it is, after all, a tank
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        //transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        GameObject.FindWithTag("Controller").GetComponent<MenuControl>().GameOver(displayScore);

        FindObjectOfType<CameraShake>().ShakeCameraHard();

    }

    public void AddScore(int newScore)
    {
        score += newScore;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "EnemyBullet":
                collision.gameObject.GetComponent<EnemyBullet>().onHit();
                die();
                break;
            case "Powerup":
            case "Bullet":
            case "PiercingBullet":
                break;
            default:
                Debug.Log("Unrecognized tag in OnTriggerEnter2D in Player! Tag: " + collision.tag);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.collider.tag)
        {
            case "Enemy":
                die();
                break;
            case "Wall":
                break;
            case "Pit":
                die();
                break;
            default:
                Debug.Log("Unknown tag collided with Player! Tag: " + collision.collider.tag);
                break;
        }
    }

    public void SetPowerupUI(string name, float duration)
    {
        transform.GetChild(3).GetChild(3).GetChild(1).GetComponent<Text>().text = name;
        currentPowerupDuration = duration;
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
                SetPowerupUI("Double Speed", time);
                break;

            //2 = firing speed
            case 2:
                ATTACK_DELAY = ATTACK_DELAY / 2;
                SetPowerupUI("Double Fire Rate", time);
                break;

            case 3:
                //piercing bullet
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

            case 2:
                ATTACK_DELAY = ATTACK_DELAY * 2;
                break;

            default:
                //Debug.LogWarning("Deactivating unrecognized powerup");
                break;
        }
        //Debug.Log("powerup expired");
        activePowerup = 0;
        powerUpTimeout = 0;
        transform.GetChild(3).GetChild(3).GetChild(1).GetComponent<Text>().text = "No Power Up";
    }

    private float dotProduct(Vector3 v1, Vector3 v2)
    {
        return (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z);
    }


}

