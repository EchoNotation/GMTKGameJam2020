using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(1).GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitGame()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void GameOver(int score)
    {
        transform.GetChild(1).GetComponent<Canvas>().enabled = true;
        transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = score.ToString();
    }

    public void CloseGameOver()
    {
        transform.GetChild(1).GetComponent<Canvas>().enabled = false;
        GetComponent<Gamecontroller>().Reset();
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
