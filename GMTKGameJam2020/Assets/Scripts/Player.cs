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
    private int speed = 10;

    //Player Input
    private Vector3 screenCenter;

    // 0 means no powerup active
    private int activePowerup = 0;
    // time left on the powerup timer
    private float powerUpTimeout = 0;

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


        //decrease active powerup
        powerUpTimeout--;
        if(powerUpTimeout <= 0)
        {
            DeactivatePowerup();
        }

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

    //yes I know I am very lazy with this code, but hey game jam - CG
    //this function is activated by a powerup
    public void ActivatePowerup(int powerup, float time)
    {
        //prevent powerup stacking, powerup = 0 means no powerup active
        if(powerup != 0)
        {
            DeactivatePowerup();
        }
        
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
                Debug.LogWarning("Deactivating unrecognized powerup");
                break;
        }

    }

}

