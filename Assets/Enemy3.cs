using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{  
    float _distance;

    Rigidbody _rb;
    Animator _anim;

    float _timerJump;

    // Bool Status
    bool _isJumped;
    bool _isDead;
    bool _isblocked;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        RbChilds(true);
        ColChilds(false);
       
        _anim.SetFloat("Speed", 1);
        _isJumped = false;
        _isDead = false;
    }
    void LateUpdate()
    {
        _distance = Vector3.Distance(GameManager.Instance._player.position, transform.position);
       
        if (_distance <= 0.8f && !_isJumped)
        {
            Player_Movement.Instance.Death();
            _anim.SetTrigger("Jump");
            _rb.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
         
            _timerJump += Time.deltaTime;
            if (_timerJump >= 0.05f)
            {
                _isJumped = true;

                transform.position = GameManager.Instance._player.position;
                Death();
            }
        }
    }


    void RbChilds(bool state)
    {
        Rigidbody[] rbc = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody childrb in rbc)
        {
            childrb.isKinematic = state;
        }
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
    void ColChilds(bool state)
    {
        Collider[] Colc = GetComponentsInChildren<Collider>();
        foreach (Collider ChildCol in Colc)
        {
            ChildCol.enabled = state;

        }
        gameObject.GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Killer"))  Death();
    }

    void Death()
    {
        _isDead = true;
        _rb.velocity = Vector3.zero;
        _anim.enabled = false;
        _rb.GetComponent<Collider>().enabled = false;
        RbChilds(false);
        ColChilds(true);
    }
}
