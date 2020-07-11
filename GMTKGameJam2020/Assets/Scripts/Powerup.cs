using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [HideInInspector]
    public Player player;

    //3 variables for the price of one action
    //powerupID is sent to player, they have to make sure to map it the same way here

    public PowerupType powerupType;

    public enum PowerupType
    {
        SPEED,
    }

    int powerupID = 0;

    public float duration = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        
        switch(powerupType)
        {
            case PowerupType.SPEED:
                powerupID = 1;
                break;
        }
        //Debug.Log("Start Powerup, " + powerupID);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("something entered the trigger");
        if(collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("That thing was the player");
            player.ActivatePowerup(powerupID, duration);

            Destroy(gameObject);
        }
    }
}

