using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public bool _isCoop = false;
    [SerializeField]
    private bool _isGameOver = false;

    private void Update()
    {
        CoopCheck();
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true && _isCoop == false)
        {
            SceneManager.LoadScene(1); //Current Game Scene
        }
        else if(Input.GetKeyDown(KeyCode.R) && _isGameOver == true && _isCoop == true)
        {
            SceneManager.LoadScene(2);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //call pause menu
            //freeze frame
        }

    }

    public void CoopCheck()
    {
        if (SceneManager.GetActiveScene().name == "Single_Player")
        {
            _isCoop = false;
            Debug.Log("Now Single Player Scene");
        }
        else if (SceneManager.GetActiveScene().name == "Co-op")
        {
            _isCoop = true;
            Debug.Log("Now in Coop mode");
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Manager :: GameOver() Called");
        _isGameOver = true;
    }

}
