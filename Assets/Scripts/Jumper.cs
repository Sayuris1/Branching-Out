using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private bool _isJump = true;
    [SerializeField] private bool _isCooldown = false;
    [SerializeField] private float _cooldownStart = 2;
    [SerializeField] private float _jumpForce = 400;
    private float _cooldownCurrent = 2;

    private void Start()
    {
        _cooldownCurrent = _cooldownStart;
    }

    private void Update()
    {
        _cooldownCurrent -= Time.deltaTime;
        if(_cooldownCurrent <= 0)
        {
            _cooldownCurrent = _cooldownStart;
            if(_isJump)
                PlayerController.Instance.Expand(transform.position, _jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Stickable") && _isJump)
        {
            _isJump = true;
        }
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
