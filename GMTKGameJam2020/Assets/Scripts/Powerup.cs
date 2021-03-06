﻿using System.Collections;
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
        FIRE,
        PIERCE,
        EXPLOSIVE,
        MULTISHOT
    }

    int powerupID = 0;

    public float duration = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        switch (powerupType)
        {
            case PowerupType.SPEED:
                powerupID = 1;
                break;
            case PowerupType.FIRE:
                powerupID = 2;
                break;
            case PowerupType.PIERCE:
                powerupID = 3;
                break;
            case PowerupType.EXPLOSIVE:
                powerupID = 4;
                break;
            case PowerupType.MULTISHOT:
                powerupID = 5;
                break;
            default:
                Debug.Log("Invalid PowerupType in Powerup! Type: " + powerupType);
                break;
        }
        //Debug.Log("Start Powerup, " + powerupID);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("something entered the trigger");
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("That thing was the player");
            player.ActivatePowerup(powerupID, duration);

            Destroy(gameObject);
        }
    }
}

