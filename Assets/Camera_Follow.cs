using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [SerializeField] Vector3 _offset;
    [SerializeField] GameObject _player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,_player.transform.position.y,_player.transform.position.z) + _offset;      
    }
}
