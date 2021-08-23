using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [SerializeField] Vector3 _offset;
    [SerializeField] Transform _player,_player2;
    [SerializeField] Vector3 _velocity=Vector3.zero;
    [SerializeField] float _speed=1;

    public bool _isCam2Active;

    private void Start()
    {
        _isCam2Active = false;
        _player = GameManager.Instance._player;
        _player2 = GameManager.Instance._player_Hips;
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance._isCam2Active) transform.position = new Vector3(0,0,_player.position.z) + _offset;
        if (GameManager.Instance._isCam2Active && !GameManager.Instance._isWinLevel)
        {  
          transform.position =  Vector3.Lerp(transform.position,new Vector3(_player2.position.x, (_player2.position.y+3),(_player2.position.z-5)),  _speed*Time.deltaTime);
        }
        if (GameManager.Instance._isCam2Active && GameManager.Instance._isWinLevel)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(_player2.position.x, (_player2.position.y + 15), (_player2.position.z - 25)), _speed * Time.deltaTime);
        }

    }
}
