using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTo : MonoBehaviour
{
    [SerializeField] private Transform _moveTo;
    [SerializeField] private float _speed;

    private Vector3 _startPoint;

    private void Start()
    {
        _startPoint = transform.position;
        _moveTo.SetParent(null);
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _moveTo.position, _speed);

        if(Vector3.Distance(transform.position, _moveTo.position) <= 0.9)
        {
            Vector3 temp = _startPoint;
            _startPoint = _moveTo.position;
            _moveTo.position = temp;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
