using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Control Types
    private bool isShootMode = false;
    public float timeToSwap;
    private static int SWAP_DURATION = 60;

    //Control Info
    private Vector3 shootDirection;
    private Vector3 moveDirection;
    public int speed = 3;

    //Player Input
    private Vector3 screenCenter;

    // Start is called before the first frame update
    void Start()
    {
        timeToSwap = SWAP_DURATION;
        screenCenter = new Vector3(Screen.width/2, Screen.height/2, 0);
        shootDirection = Vector3.forward;
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 direction = Input.mousePosition - screenCenter;
            OnInput(direction);
        }

        //Automatic Controls
        this.gameObject.transform.Translate(moveDirection * speed * Time.deltaTime);



    }

    private void OnInput(Vector2 direction)
    {
        if (isShootMode)
        {
            shootDirection = direction.normalized;
            Debug.Log(shootDirection);
            this.gameObject.transform.GetChild(0).eulerAngles = new Vector3(0,0, Vector3.Angle(new Vector3(1, 0, 0), shootDirection));
        }
        else
        {
            moveDirection = direction.normalized;
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
            default:
                Debug.Log("Unrecognized tag in OnTriggerEnter2D in Player! Tag: " + collision.tag);
                break;
        }
    }


}
