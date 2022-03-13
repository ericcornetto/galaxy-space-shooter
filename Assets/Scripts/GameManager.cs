using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;

    public bool _isCoopMode = false;

    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private Animator _pauseMenuAnim;

    void Start()
    {
        _pauseMenuAnim = _pauseMenu.GetComponent<Animator>();

        if(_pauseMenu == null)
        {
            Debug.Log("Pause Menu is Null");
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && _isGameOver == true && _isCoopMode == false)
        {
            SceneManager.LoadScene(1);

        } else if(Input.GetKeyDown(KeyCode.R) && _isGameOver == true && _isCoopMode == true)
        {
            SceneManager.LoadScene(2);
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            _pauseMenu.SetActive(true);
            _pauseMenuAnim.SetBool("isPaused", true);
            PauseGame();
        }

    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _pauseMenuAnim.SetBool("isPaused", false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        ResumeGame();
    }
}
