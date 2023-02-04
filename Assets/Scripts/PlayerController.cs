using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _forceToAdd;

    private Rigidbody2D _rigitBody;
    private Stick _stick;
    

    #region Life Cycle
    private void Start()
    {
        _stick = GetComponent<Stick>();
        _rigitBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
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

    #endregion

    #region Movement
    private void AddForce(float force)
    {
        _stick.AddTorque(force);
    }
    #endregion
}
