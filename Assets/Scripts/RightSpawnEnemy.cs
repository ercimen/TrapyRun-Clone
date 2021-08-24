using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightSpawnEnemy : MonoBehaviour
{
    [SerializeField] Data _data = null; // For speed
    [SerializeField] bool _isRight;
    float _maxSpeed;
    float _distance;

    Rigidbody _rb;
    Animator _anim;

    float _timerJump;
    float _randomStartPos;
    // Bool Status
    bool _isJumped;
    bool _isDead;
    bool _isStart;
    bool _isMainRoad;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        RbChilds(true);
        ColChilds(false);
        _maxSpeed = 1.1f;
        _anim.SetFloat("Speed", 1);
        _isJumped = false;
        _isDead = false;

        if (_isRight)
        {
            _randomStartPos = Random.Range(5, 8);
        }
        else
        {
            _randomStartPos = Random.Range(0, 4);
        }

    }

    private void Start()
    {
        GameManager.StartGame += StartNow;
    }

    void FixedUpdate()
    {
        if (_isStart)
        {
            GroundCheck();
            if (_maxSpeed <= 1) _maxSpeed = 1f;
            if (!_isDead && !_isJumped && !_isMainRoad) _rb.velocity = transform.forward * _data._speed * 0.45f * Time.deltaTime * _maxSpeed;
            if (!_isDead && !_isJumped && _isMainRoad) _rb.velocity = transform.forward * _data._speed * Time.deltaTime * _maxSpeed;
        }
    }

    void LateUpdate()
    {
        JumpAttack();
        GotoMainRoad();
    }

    void GotoMainRoad()
    {
        if (_isRight)
        {
            if (transform.position.x <= _randomStartPos && !_isMainRoad)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                _isMainRoad = true;
            }
        }
        else
        {
            if (transform.position.x >= _randomStartPos && !_isMainRoad)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                _isMainRoad = true;
            }
        }
    }

    void JumpAttack()
    {
        _distance = Vector3.Distance(GameManager.Instance._player.position, transform.position);

        if (GameManager.Instance._player.position.z - transform.position.z > 1)
        {
            _maxSpeed = ((_distance * 0.07f) + 1.1f);
        }
        else
        {
            _maxSpeed = 1;
        }

        if (_distance <= 0.8f && !_isJumped)
        {
            Player_Movement.Instance.Death();
            transform.LookAt(GameManager.Instance._player.position);
            _anim.SetTrigger("Jump");
            _rb.velocity = Vector3.zero;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(GameManager.Instance._player.position.x, 2.5f, GameManager.Instance._player.position.z), Time.deltaTime);

            _timerJump += Time.deltaTime;
            if (_timerJump >= 0.10f)
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
        if (other.CompareTag("Killer")) Death();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Killer")) Death();
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
    void DeathFall()
    {
        _isDead = true;
        StartCoroutine(DeathTime(0.1f));

    }

    IEnumerator DeathTime(float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        _anim.enabled = false;
        _rb.GetComponent<Collider>().enabled = false;
        RbChilds(false);
        ColChilds(false);
    }

    void GroundCheck()
    {

        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1f, 0f);
        Vector3 dir2 = new Vector3(0, -1f, 0.3f);

        if (!Physics.Raycast(transform.position, dir, out hit, distance))
        {
            if (!Physics.Raycast(transform.position + new Vector3(0.1f, 0, 0.45f), dir2, out hit, distance))
            {
                DeathFall();
            }

        }

    }
    void StartNow(bool status)
    {
        _isStart = status;
    }
}
