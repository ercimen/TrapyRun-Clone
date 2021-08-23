using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliCopterWinAnimation : MonoBehaviour
{
    Animator _anim;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            _anim.SetTrigger("win");
        }
    }

}
