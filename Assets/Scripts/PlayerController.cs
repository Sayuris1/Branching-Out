using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _forceToAdd;
    [SerializeField] private float grabBoost = 2f;

    [SerializeField] private float expansionForce = 10f;
    [SerializeField] private float _addOrthoValuePerRoot = 1f;

    private Rigidbody2D _rigitBody;
    private Stick _stick;
    public float expandCooldownTime;
    private float _expandCooldownTimer = 0;
    [SerializeField] private float maxExpandMult = 2f;

    public static PlayerController Instance;
    public bool isGrabbing;
    private int allSticksCount = 0;

    private float _currentOrthoScale;
    [SerializeField] private CinemachineVirtualCamera cam;

    private void Awake()
    {
        Instance = this;
    }

    public float GetCooldownPercent()
    {
        if (_expandCooldownTimer < 0)
            return 0;
        return _expandCooldownTimer / expandCooldownTime;
    }

    #region Life Cycle
    private void Start()
    {
        _stick = GetComponent<Stick>();
        _rigitBody = GetComponent<Rigidbody2D>();

        allSticksCount = FindObjectsOfType<Stick>().Length;

        _currentOrthoScale = cam.m_Lens.OrthographicSize;
    }

    public void AddToOrthoScale()
    {
        _currentOrthoScale += _addOrthoValuePerRoot;
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
            Expand(Camera.main.ScreenToWorldPoint(Input.mousePosition), expansionForce);
        }

        _expandCooldownTimer -= Time.deltaTime;
       SetMusicVolume();

       cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, _currentOrthoScale, 0.001f);
    }

    private void SetMusicVolume()
    {
        int myStickCount = _stick.GetAllChilds().Count;
        myStickCount++;
        //LayeredMusicPlayer.Instance.musicPercent = myStickCount / (float)allSticksCount;
    }

    public void Expand(Vector3 pos, float force)
    {
        List<Stick> childs = new List<Stick>();
        childs.Add(_stick);
        childs.AddRange(_stick.GetAllChilds());
        Vector3 center = pos;
        
        // foreach (var child in childs)
        // {
        //     center += child.transform.position;
        // }

        // center /= childs.Count;
        foreach (var child in childs)
        {
            Vector2 vec = child.transform.position - center;
            Vector2 addForce = Mathf.Clamp(1 / vec.magnitude, 0, maxExpandMult) * vec.normalized;
            
            child.AddForce(addForce * force);
        }
    }

    public void ExpandTowards(Vector3 towards, float force)
    {
        List<Stick> childs = new List<Stick>();
        childs.Add(_stick);
        childs.AddRange(_stick.GetAllChilds());
        
        // foreach (var child in childs)
        // {
        //     center += child.transform.position;
        // }

        // center /= childs.Count;
        foreach (var child in childs)
        {
            child.AddForce(towards * force);
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
