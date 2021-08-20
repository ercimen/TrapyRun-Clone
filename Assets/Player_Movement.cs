using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] Data _data = null;
    Rigidbody _rb;
    Animator _anim;

    int _fallBoxNumber;
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        RbChilds(false);
        ColChilds(false);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = transform.forward * _data._speed * Time.deltaTime;
        Move();
    }

    private void Move()
    {
        _anim.SetFloat("Speed", 1);
        float y = Input.GetAxis("Horizontal");


        if (y ==0)
        {
          
            transform.localEulerAngles = (new Vector3(0, 0, 0));
        }

        if (y > 0.1f)
            {
             
                transform.localEulerAngles = (new Vector3(0, 45f, 0));
            }
            if (y < -0.1f)
            {
          
            transform.localEulerAngles = (new Vector3(0, -45f, 0));
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


    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
             _fallBoxNumber++;
            if (_fallBoxNumber>=Object_Pool.Instance.FallBox.Length) _fallBoxNumber = 0;
            Object_Pool.Instance.FallBox[_fallBoxNumber].SetActive(true);
            collision.collider.gameObject.isStatic = false;
            collision.collider.gameObject.SetActive(false);
            Object_Pool.Instance.FallBox[_fallBoxNumber].gameObject.transform.position = collision.collider.gameObject.transform.position;
            StartCoroutine(Falling(0.5f,_fallBoxNumber));
        }
    }

    IEnumerator Falling(float _wait,int _currentNo)
    {
     //   yield return new WaitForSecondsRealtime(_wait);
            Object_Pool.Instance.FallBox[_currentNo].GetComponent<Rigidbody>().isKinematic = false;
            Object_Pool.Instance.FallBox[_currentNo].GetComponent<Animator>().enabled = true;
        yield return new WaitForSecondsRealtime(_wait*4);
            Object_Pool.Instance.FallBox[_currentNo].SetActive(false);
            Object_Pool.Instance.FallBox[_currentNo].GetComponent<Animator>().enabled = false;
            Object_Pool.Instance.FallBox[_currentNo].GetComponent<Rigidbody>().isKinematic = true;
    }
}
