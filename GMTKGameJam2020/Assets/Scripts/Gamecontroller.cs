﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamecontroller : MonoBehaviour
{
    private int gameScore, waveCount, enemyCount;
    private float halfWidth;
    public Camera mainCamera;
    public GameObject Charger, Gunner, Bombmer;
    public bool gamePaused;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        halfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        Reset();
        for (int i = 0; i < 10; i++) {
            SpawNextWave();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gamePaused) {
            //TODO:add player and bullet updates?
        }
    }

    void GoToScene() {
        //TODO: What should be passed here
        //TODO: Make scenes (Title, Game, score – could be overlay, paused - could be overlay, Credits)
    }

    void Reset() {
        gameScore = 0;
        waveCount = 0;
        Pause();
    }

    void Pause() {
        gamePaused = true;
    }

    public void Play() {
        gamePaused = false;
    }

    void SpawNextWave() {
        Instantiate(Gunner, SpawnLocation(), Quaternion.identity);
    }

    private Vector2 SpawnLocation() {
        Vector2 Direction_Vector = Vector2.zero;
        while (Direction_Vector == Vector2.zero) {
            Direction_Vector = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        }
        Direction_Vector = Direction_Vector.normalized;
        float Magnitude = Random.Range(halfWidth, halfWidth * 6);

        //TODO: Generate random offscreen coordinates, spaced from a previous coord set
        return Direction_Vector * Magnitude;
    }

    public void RegisterEnemy(GameObject enemy) {
        //TODO: track totoal and current enemies to know when to spwan next wave
        enemyCount++;
    }
    public void RemoveEnemy(GameObject enemy) {
        enemyCount--;
    }
}
