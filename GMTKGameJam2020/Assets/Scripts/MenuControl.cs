using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        transform.GetComponent<Gamecontroller>().Play();
    }

}
