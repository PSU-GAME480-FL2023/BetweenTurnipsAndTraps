using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Sprite startDown;
    public Sprite quitDown;

    public void OnStartPress()
    {
        SceneManager.LoadScene(3);
    }

    public void OnQuitPress()
    {
        Application.Quit();
    }
}
