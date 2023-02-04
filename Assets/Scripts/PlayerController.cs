using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _forceToAdd;

    private Rigidbody2D _rigitBody;

    #region Life Cycle
    private void Start()
    {
        _rigitBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            AddForce(_forceToAdd);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            AddForce(-_forceToAdd);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Stickable")
            return;


        HingeJoint2D addedJoint =  collision.gameObject.AddComponent<HingeJoint2D>();
        addedJoint.connectedBody = _rigitBody;
        addedJoint.enableCollision = true;
        addedJoint.anchor = collision.contacts[0].point;
        addedJoint.connectedAnchor = collision.contacts[0].point;
    }
    #endregion

    #region Movement
    private void AddForce(float force)
    {
        _rigitBody.AddTorque(force);
    }
    #endregion
}
