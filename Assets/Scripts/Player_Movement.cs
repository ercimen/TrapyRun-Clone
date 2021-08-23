using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{

    [SerializeField] Data _data = null; // For speed

    // Move Variables
    float _firstPressPos;
    float _secondPressPos;
    float _currentSwipe;
    Rigidbody _rb;
    Animator _anim;

    // For Box Pooling
    int _fallBoxNumber;


    // Bool Status
    bool _isAnimated;
    bool _isDead;
    bool _isblocked;

    #region Singleton

    private static Player_Movement _instance;

    public static Player_Movement Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Player_Movement>();
            return _instance;
        }
    }
    #endregion

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        RbChilds(true);
        ColChilds(false);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance._isGameStarted)
        {
            if (!_isDead) _rb.velocity = transform.forward * _data._speed * Time.deltaTime;
            //  if (_isblocked) _rb.velocity = transform.forward * _data._speed * Time.deltaTime*0.5f;
        }
        else if (GameManager.Instance._isWinLevel)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
          _rb.velocity = transform.forward * _data._speed * Time.deltaTime;
        }

    }

    private void Update()
    {
        if (GameManager.Instance._isGameStarted)
        {
            if (!_isDead) Swipe();
        }
    }


    public void Swipe()
    {
        if (!_isAnimated)
        {
            _anim.SetFloat("Speed", 1);
            _isAnimated = true;
        }
        if (Input.GetMouseButtonDown(0)) _firstPressPos = Input.mousePosition.x;

        if (Input.GetMouseButton(0))
        {
            _secondPressPos = Input.mousePosition.x;
            _currentSwipe = _secondPressPos - _firstPressPos;

            if (_currentSwipe < 0) transform.localRotation = Quaternion.Euler(0, -45, 0);

            if (_currentSwipe > 0) transform.localRotation = Quaternion.Euler(0, 45, 0); ;
        }

        if (Input.GetMouseButtonUp(0)) transform.localRotation = Quaternion.Euler(0, 0, 0); ;

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
        if (other.CompareTag("Killer")) Death();
        if (other.CompareTag("Enemy")) Death();
        if (other.CompareTag("Finish"))
        {
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true); // Confeti
            GameManager.Instance.NextLevel();
        }
            
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Killer")) Death();
        if (collision.collider.CompareTag("Enemy")) Death();
    }



    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") && !_isDead)
        {
            _fallBoxNumber++;
            if (_fallBoxNumber >= Object_Pool.Instance.FallBox.Length) _fallBoxNumber = 0;
            Object_Pool.Instance.FallBox[_fallBoxNumber].SetActive(true);
            Object_Pool.Instance.Notwalk[_fallBoxNumber].SetActive(true);
            collision.collider.gameObject.isStatic = false;
            collision.collider.gameObject.SetActive(false);
            Object_Pool.Instance.FallBox[_fallBoxNumber].gameObject.transform.position = collision.collider.gameObject.transform.position;
            Object_Pool.Instance.Notwalk[_fallBoxNumber].gameObject.transform.position = collision.collider.gameObject.transform.position + new Vector3(0, 0.5f, 0);
            StartCoroutine(Falling(0.5f, _fallBoxNumber));
        }

    }

    IEnumerator Falling(float _wait, int _currentNo) // Falling Boxes
    {
        //   yield return new WaitForSecondsRealtime(_wait);
        Object_Pool.Instance.FallBox[_currentNo].GetComponent<Rigidbody>().isKinematic = false;
        Object_Pool.Instance.FallBox[_currentNo].GetComponent<Animator>().enabled = true;
        yield return new WaitForSecondsRealtime(_wait * 5);
        Object_Pool.Instance.FallBox[_currentNo].SetActive(false);
        Object_Pool.Instance.Notwalk[_currentNo].SetActive(false);
        Object_Pool.Instance.FallBox[_currentNo].GetComponent<Animator>().enabled = false;
        Object_Pool.Instance.FallBox[_currentNo].GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Death()
    {
        _rb.GetComponent<Collider>().enabled = false;
        GameManager.Instance.Death();

        _isDead = true;
        _anim.enabled = false;

        RbChilds(false);
        ColChilds(true);
    }
}
