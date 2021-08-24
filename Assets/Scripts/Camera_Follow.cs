using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [SerializeField] Vector3 _offset;
    [SerializeField] Transform _player, _player2;
    [SerializeField] float _speed = 1;

    public string _camera;

    private void Start()
    {
        GameManager.Cam2Active += GoCam2;
    }
    // Update is called once per frame
    void Update()
    {
        if (_camera == "DeathCam")
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(_player2.position.x, (_player2.position.y + 3), (_player2.position.z - 5)), _speed * Time.deltaTime);
        }
        else if (_camera == "WinCam")
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(_player2.position.x, (_player2.position.y + 15), (_player2.position.z - 25)), _speed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(0, 0, _player.position.z) + _offset;
        }

    }

    void GoCam2(string status)
    {
        _camera = status;
        Debug.Log(_camera);
    }
}
