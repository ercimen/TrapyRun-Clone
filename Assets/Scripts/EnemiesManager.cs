using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesManager : MonoBehaviour
{
    int _enemyNo,_enemyNo2,_enemyNo3;
    [SerializeField] float _spawmTime=1;

    bool _isStart;
    void Start()
    {
        GameManager.StartGame += StartNow;
        InvokeRepeating("CreateEnemy", _spawmTime, _spawmTime);

        for (int i = 0; i < 20; i++)
        {
            Object_Pool.Instance.Enemies1[_enemyNo].transform.position = new Vector3(Random.Range(0, 9), 0.86f, Random.Range(0, 10));
            Object_Pool.Instance.Enemies1[_enemyNo].SetActive(true);
            _enemyNo++;
        }

        if (SceneManager.GetActiveScene().buildIndex>0)
        {
            for (int i = 0; i < 5; i++)
            {
                Object_Pool.Instance.Enemies2[_enemyNo2].transform.position = new Vector3(Random.Range(0, 9), 0.86f, Random.Range(0, 10));
                Object_Pool.Instance.Enemies2[_enemyNo2].SetActive(true);
                _enemyNo2++;
            }
        }
    }
    void CreateEnemy()
    {
        if (_isStart)
        {
            int _rnd = Random.Range(0, 100);
          
            if (_rnd < 65)
            {
                float _randomX = Random.Range(0, 9);
                float _randomZ = Random.Range(0, 10);

                Object_Pool.Instance.Enemies1[_enemyNo].transform.position = new Vector3(_randomX, 0.86f,GameManager.Instance._player.position.z-15);
                Object_Pool.Instance.Enemies1[_enemyNo].SetActive(true);
                _enemyNo++;
            }
            if (_rnd >64 && _rnd < 96)
            {
                float _randomX = Random.Range(0, 9);
                float _randomZ = Random.Range(0, 10);

                Object_Pool.Instance.Enemies2[_enemyNo2].transform.position = new Vector3(_randomX, 1, GameManager.Instance._player.position.z - 15);
                Object_Pool.Instance.Enemies2[_enemyNo2].SetActive(true);
                _enemyNo2++;
            }
            if (_rnd > 95 )
            {
                float _randomX = Random.Range(0, 9);
                float _randomZ = Random.Range(0, 10);

                Object_Pool.Instance.Enemies3[_enemyNo3].transform.position = new Vector3(_randomX, 1, GameManager.Instance._player.position.z - 15);
                Object_Pool.Instance.Enemies3[_enemyNo3].SetActive(true);
                _enemyNo3++;
            }

        }
    }
    void StartNow(bool status)
    {
        _isStart = status;
    }
}
