using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HideQuit : MonoBehaviour
{

    public GameObject quitButton;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform != RuntimePlatform.WindowsPlayer)
        {
            quitButton.SetActive(false);
        }
    }


}
