using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void LoadSinglePlayer()
    {
        SceneManager.LoadScene(1); //Game Scene
    }
    public void LoadCoop()
    {
        SceneManager.LoadScene(2); //Game Scene
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
