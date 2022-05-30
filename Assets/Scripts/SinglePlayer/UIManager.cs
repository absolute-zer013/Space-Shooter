using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _finalScore;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprites;
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
        _finalScore.gameObject.SetActive(false);
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

    public void FinalScore(int final)
    {
        _finalScore.text = "Your Final Score is " + final;
    }

    public void UpdateLives(int currentLives)
    {
        //display img sprite
        //give new based on the currentlives index
        _livesImg.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
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
        _finalScore.gameObject.SetActive(true);
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
