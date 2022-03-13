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
    private Sprite[] _liveSprites;

    [SerializeField]
    private Image _livesDisplay;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private Text _bestScore;


    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _bestScore.text = "Best Score: " + PlayerPrefs.GetString("Best Score", "0");

        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }



    public void UpdatedLives(int playerLives)
    {
        _livesDisplay.sprite = _liveSprites[playerLives];

        if(playerLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateBestScore()
    {
        string[] currentScore = _scoreText.text.Split(' ');
        string[] bestScore = _bestScore.text.Split(' ');

        if(int.Parse(currentScore[1]) > int.Parse(bestScore[2]))
        {
            _bestScore.text = "Best Score: " + currentScore[1].ToString();
            PlayerPrefs.SetString("Best Score", currentScore[1]);
        }
    }

    private void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        UpdateBestScore();
        StartCoroutine(GameOverFlickerRoutine());
        _gameManager.GameOver();

    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
