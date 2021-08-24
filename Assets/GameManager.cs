using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }
    #endregion

    [Header("Player")]
    public Transform _player;
    public Transform _player_Hips;
    public bool _isPlayerDead;

    [Header("Cameras")]
    public GameObject Camera1;
    public GameObject Camera2;
   

    [Header("Panels")]
    public GameObject inGamePanel;
    public GameObject GameOverPanel;
    public GameObject NextLevelPanel;
    public GameObject TapToPlay;
    [Header("Other")]
    public Text LevelTxt;
    int _levelno;

   
    public bool _isWinLevel;
    bool click; //nextlevel doubleclick check

    public delegate void GameStart(bool status);
    public static event GameStart StartGame;
    public delegate void Cam2(String cam);
    public static event Cam2 Cam2Active;

    private void Awake()
    {
        inGamePanel.gameObject.SetActive(false);
        TapToPlay.gameObject.SetActive(true);
    }
    private void Start()
    {
        click = false;
        if (PlayerPrefs.HasKey("level"))
        {
            _levelno = PlayerPrefs.GetInt("level");
        }
        else
        {
            PlayerPrefs.SetInt("level", 1);
            _levelno = 1;
        }

        LevelTxt.text = _levelno.ToString();
        Cam2Active("FirstCam");
        Camera1.gameObject.SetActive(true);
        Camera2.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        inGamePanel.SetActive(false);
        GameOverPanel.SetActive(true);
        StartGame(false);
    }
    public void NextLevel()
    {
        inGamePanel.SetActive(false);
        NextLevelPanel.SetActive(true);
        Cam2Active("WinCam");
        _isWinLevel = true;
        StartGame(false);
        Camera1.gameObject.SetActive(false);
        Camera2.transform.position = Camera1.transform.position;
        Camera2.gameObject.SetActive(true);
    }
    public void NextLevelButton()
    {
        StartGame(true);
        if (!click)
        {  
            _levelno ++;
            StartCoroutine(ResourceTickOver(.5f, 1));
            click = true;
            inGamePanel.gameObject.SetActive(true);
            PlayerPrefs.SetInt("level", _levelno);         
        }
    }
    IEnumerator ResourceTickOver(float waitTime, int level)
    {
        yield return new WaitForSeconds(waitTime);
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + level);
        }

    }

    public void TaptoPlay()
    {
        StartGame(true);
        TapToPlay.gameObject.SetActive(false);
        inGamePanel.gameObject.SetActive(true);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Death()
    {
        if (!_isPlayerDead)
        {
            GameOver();
            Cam2Active("DeathCam");
            _isPlayerDead = true;
            Camera1.gameObject.SetActive(false);
            Camera2.transform.position = Camera1.transform.position;
            Camera2.gameObject.SetActive(true);
        }
    }
}