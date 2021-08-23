using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropTurn : MonoBehaviour
{
    [SerializeField] Data _data = null; // For speed
    [SerializeField] float _waitTimeForStart = 1;
    private void FixedUpdate()
    {
        _waitTimeForStart -= Time.deltaTime;
        if (_waitTimeForStart<=0)transform.Rotate(new Vector3(0,_data._propSpeed , 0));  
    }
}
