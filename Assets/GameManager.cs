using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public Transform _player;
    public Transform _player_Hips;
    public bool _isPlayerDead;

    [Header("Cameras")]
    public GameObject Camera1;
    public GameObject Camera2;
    public bool _isCam2Active;

    [Header("Panels")]
    public GameObject inGamePanel;
    public GameObject GameOverPanel;
    public GameObject NextLevelPanel;
    public GameObject TapToPlay;
    [Header("Other")]
    public Text LevelTxt;
    int _levelno;

    public bool isGameStarted;
    bool click; //nextlevel doubleclick check


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
    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            isGameStarted = false;
        }
        inGamePanel.gameObject.SetActive(false);


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
        _isCam2Active = false;
        Camera1.gameObject.SetActive(true);
        Camera2.gameObject.SetActive(false);

    }

    public void GameOver()
    {
        inGamePanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }
    public void NextLevel()
    {
        inGamePanel.SetActive(false);
        NextLevelPanel.SetActive(true);
    }
    public void NextLevelButton()
    {
       
        isGameStarted = true;
        if (!click)
        {
            StartCoroutine(ResourceTickOver(.5f, 1));
            click = true;
            inGamePanel.gameObject.SetActive(true);
        }
    }
    IEnumerator ResourceTickOver(float waitTime, int level)
    {
        yield return new WaitForSeconds(waitTime);
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + level);
        }

    }
  
    public void TapTap()
    {
        isGameStarted = true;
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
        _isCam2Active = true;
        _isPlayerDead = true;
        Camera1.gameObject.SetActive(false);
        Camera2.transform.position = Camera1.transform.position;
        Camera2.gameObject.SetActive(true);
        }
    }

}