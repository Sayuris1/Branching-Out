using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private bool _isJump = true;
    [SerializeField] private float _cooldownStart = 2;
    [SerializeField] private float _jumpForce = 400;
    private float _cooldownCurrent = 2;


    private void Start()
    {
        _cooldownCurrent = _cooldownStart;
        GetComponentInChildren<Animator>().SetBool("idle", true);
    }

    private void Update()
    {
        _cooldownCurrent -= Time.deltaTime;
        if(_cooldownCurrent <= 0)
        {
            _cooldownCurrent = _cooldownStart;
            if(_isJump)
            {
                Debug.Log("ASdASD");
                GetComponentInChildren<Animator>().SetBool("idle", false);
                PlayerController.Instance.Expand(transform.position, _jumpForce);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"NAME {other.gameObject.name}  sitck {other.gameObject.GetComponent<Stick>().sticky}");
        if(other.gameObject.CompareTag("Stickable") && _isJump && other.gameObject.GetComponent<Stick>().sticky)
            _isJump = true;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        _isJump = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _isJump = false;
    }
}
