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
        if (!gamePaused) {
            //TODO:add player and bullet updates?
        }
    }

    void GoToScene() {
        //TODO: What should be passed here
        //TODO:Make scenes (Title, Game, score – could be overlay, paused - could be overlay, Credits)
    }

    void ExitGame() {
        if(Application.platform == RuntimePlatform.WindowsPlayer) {
            Application.Quit();
        }
    }
    void CalcuateScore() {

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

    void SpawNextWave() {
        
    }
}
