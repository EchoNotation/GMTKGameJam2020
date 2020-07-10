using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamecontroller : MonoBehaviour
{
    private int gameScore;
    public bool gamePaused;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IncrementScore() {

    }

    int GetScore() {
        return gameScore;
    }

    void Reset() {
        gameScore = 0;
        Pause();
    }

    void Pause() {
        gamePaused = true;
    }

    void Play() {
        gamePaused = false;
    }

}
