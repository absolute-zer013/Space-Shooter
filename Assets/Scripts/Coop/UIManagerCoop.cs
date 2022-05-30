using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManagerCoop : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _finalScoreCoop;
    [SerializeField]
    private Image _livesImgP1;
    [SerializeField]
    private Image _livesImgP2;
    [SerializeField]
    private Sprite[] _livesSpritesP1;
    [SerializeField]
    private Sprite[] _livesSpritesP2;
    [SerializeField]
    private Text _GameOverText;
    [SerializeField]
    private Text _RestartGame;
    [SerializeField]
    private Text _Instruction;
    [SerializeField]
    private Text _StartInstruction;
    public static bool GameIsPaused = false;
    [SerializeField]
    private GameObject _PauseUI;

    private GameManager _gameManager;

    // Start is called before the first frame update

    void Start()
    {
        Time.timeScale = 1f;
        _PauseUI.SetActive(false);
        StartCoroutine(StartScreen());
        _finalScoreCoop.gameObject.SetActive(false);
        _GameOverText.gameObject.SetActive(false);
        _scoreText.text = "Score: " + 0;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("404::Game Manager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Time.timeScale = 1f;
                Resume();
            }
            else
            {
                Time.timeScale = 0f;
                Pause();
            }
        }
    }
    public void Resume()
    {
        _PauseUI.SetActive(false);
        GameIsPaused = false;
    }

    void Pause()
    {
        _PauseUI.SetActive(true);
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ScoreUpdate(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void FinalScore(int finalCoop)
    {
        _finalScoreCoop.text = "Your Final Score is " + finalCoop;
    }
    

    public void UpdateLivesP1(int currentLivesP1)
    {
        //display img sprite
        //give new based on the currentlives index
        _livesImgP1.sprite = _livesSpritesP1[currentLivesP1];

        if (currentLivesP1 == 0)
        {
            GameOver();
        }

    }
    public void UpdateLivesP2(int currentLivesP2)
    {
        //display img sprite
        //give new based on the currentlives index
        _livesImgP2.sprite = _livesSpritesP2[currentLivesP2];

        if (currentLivesP2 == 0)
        {
            GameOver();
        }

    }

    IEnumerator StartScreen()
    {
        _Instruction.gameObject.SetActive(true);
        _StartInstruction.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        _Instruction.gameObject.SetActive(false);
        _StartInstruction.gameObject.SetActive(false);
    }
    void GameOver()
    {   
        _finalScoreCoop.gameObject.SetActive(true);
        _gameManager.GameOver();
        _RestartGame.gameObject.SetActive(true);
        StartCoroutine(GameOverLoopRoutine());
    }

    IEnumerator GameOverLoopRoutine()
    {
        while(true)
        {
            _GameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _GameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

}
