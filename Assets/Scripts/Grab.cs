using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class Grab : MonoBehaviour
{
    private HingeJoint2D _hinge;
    private bool _isGrab;
    private Vector3 point;
    private void Start()
    {
        _hinge = GetComponent<HingeJoint2D>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isGrab = false;
            _hinge.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
            _isGrab = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Stickable") && _isGrab)
        {
            _hinge.enabled = true;
            _hinge.connectedBody = other.gameObject.GetComponent<Rigidbody2D>();

            _isGrab = false;
        }
    }
}
