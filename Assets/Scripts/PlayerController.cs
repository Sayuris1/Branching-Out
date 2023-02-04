using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _forceToAdd;
    [SerializeField] private float grabBoost = 2f;

    
    [SerializeField] private float expansionForce = 10f;

    private Rigidbody2D _rigitBody;
    private Stick _stick;
    public float expandCooldownTime;
    private float _expandCooldownTimer = 0;
    [SerializeField] private float maxExpandMult = 2f;

    public static PlayerController Instance;
    public bool isGrabbing;
    private int allSticksCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    #region Life Cycle
    private void Start()
    {
        _stick = GetComponent<Stick>();
        _rigitBody = GetComponent<Rigidbody2D>();

        allSticksCount = FindObjectsOfType<Stick>().Length;
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

    private void Update()
    {
        if (_expandCooldownTimer <= 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            _expandCooldownTimer = expandCooldownTime;
            Expand();
        }

        _expandCooldownTimer -= Time.deltaTime;
       SetMusicVolume();
    }

    private void SetMusicVolume()
    {
        int myStickCount = _stick.GetAllChilds().Count;
        myStickCount++;
        LayeredMusicPlayer.Instance.musicPercent = myStickCount / (float)allSticksCount;
    }

    private void Expand()
    {
        List<Stick> childs = new List<Stick>();
        childs.Add(_stick);
        childs.AddRange(_stick.GetAllChilds());
        Vector3 center = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // foreach (var child in childs)
        // {
        //     center += child.transform.position;
        // }

        // center /= childs.Count;
        foreach (var child in childs)
        {
            Vector2 vec = child.transform.position - center;
            Vector2 force = Mathf.Clamp(1 / vec.magnitude, 0, maxExpandMult) * vec.normalized;
            
            child.AddForce(force * expansionForce);
        }
    }

    #endregion

    #region Movement
    private void AddForce(float force)
    {
        if (isGrabbing)
            force *= grabBoost;
        _stick.AddTorque(force);
    }
    #endregion
}
