using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stick : MonoBehaviour
{
    public bool sticky = false;
    [SerializeField] private bool startSticky;
    [SerializeField] private float defaultAngleForce;
    
    private Rigidbody2D _rb;
    [SerializeField] private CapsuleCollider2D capsuleCol;
    [SerializeField] private CircleCollider2D topCol;
    [SerializeField] private HingeJoint2D topHinge;
    
    [SerializeField] private CircleCollider2D bottomCol;
    [SerializeField] private HingeJoint2D bottomHinge;

    [SerializeField] private float childTorqueFactor = 1f;

    [SerializeField] private AudioSource moveAudio;
    [SerializeField] private AudioClip[] moveSounds;
    [SerializeField] private float fullMoveAudioSoundSpeed;
    [SerializeField] private AudioSource grepAudio;
    
    public List<Stick> childs = new List<Stick>();
    private JointMotor2D _topMotor;
    private JointMotor2D _bottomMotor;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _topMotor = topHinge.motor;
        _bottomMotor = bottomHinge.motor;
    }

    private void Start()
    {
        SetSticky(startSticky);
        //moveAudio.clip = moveSounds[Random.Range(0, moveSounds.Length)];
        //moveAudio.volume = 0;
        //Invoke("StartAudio", Random.Range(0, moveAudio.clip.length));
    }

    private void StartAudio()
    {
        moveAudio.Play();
    }

    public List<Stick> GetAllChilds()
    {
        List<Stick> newChilds = new List<Stick>();
        newChilds.AddRange(childs);
        foreach (var child in childs)
            newChilds.AddRange(child.GetAllChilds());
        return newChilds;
    }

    public void AddForce(Vector3 force)
    {
        _rb.AddForce(force);
    }

    private void FixedUpdate()
    {
        if (sticky)
        {
            NormalizeHinge(bottomHinge, _bottomMotor);
            NormalizeHinge(topHinge, _topMotor);
        }
    }

    private void Update()
    {
        //moveAudio.volume = _rb.velocity.magnitude / fullMoveAudioSoundSpeed;
    }

    private void NormalizeHinge(HingeJoint2D hinge, JointMotor2D motor)
    {
        if (!hinge.connectedBody)
            return;
        float force = hinge.jointAngle * defaultAngleForce;
        
        _rb.AddTorque(force);
        hinge.connectedBody.AddTorque(-force);
    }

    public void SetSticky(bool sticky)
    {
        this.sticky = sticky;

        topCol.enabled = sticky;
        bottomCol.enabled = sticky;
        // capsuleCol.enabled = !sticky;
    }

    public void OnSomeoneStick(Stick parent)
    {
        SetSticky(true);
    }

    public void StickToTarget(HingeJoint2D hinge, Stick target)
    {
        //grepAudio.Play();
        hinge.enabled = true;
        hinge.connectedBody = target._rb;
        target.OnSomeoneStick(this);
        Vector2 hingeConnectPoint = transform.TransformPoint(hinge.anchor);
        ParticleManager.Instance.Play(0, hingeConnectPoint);
        // target.transform.position = hingeConnectPoint;
        childs.Add(target);
    }

    public bool IsChildOfThis(Stick stick)
    {
        foreach (var child in childs)
        {
            if (stick == child || child.IsChildOfThis(stick))
                return true;
        }

        return false;
    }

    public void AddTorque(float torque)
    {
        _rb.AddTorque(torque);
        foreach (var child in childs)
        {
            child.AddTorque(torque * childTorqueFactor);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        Gizmos.color = Color.yellow;
        
        foreach (var child in childs)
        {
            Gizmos.DrawLine(transform.position, child.transform.position);
        }
    }

    private bool CanConnectToCollider(Collider2D col, Stick target)
    {
        if (col == target.bottomCol || col == target.topCol)
            return false;
        if (target.sticky)
            return false;
        if (IsChildOfThis(target))
            return false;
        if (target.IsChildOfThis(this))
            return false;
        return true;
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        ParticleManager.Instance.Play(1, col.contacts[0].point);
        if (!sticky)
            return;
        Stick target = col.gameObject.GetComponent<Stick>();
        if (!target)
            return;
        if (!CanConnectToCollider(col.otherCollider, target))
            return;
        if (!topHinge.connectedBody && col.otherCollider == topCol)
            StickToTarget(topHinge, target);
        else if (!bottomHinge.connectedBody && col.otherCollider == bottomCol)
            StickToTarget(bottomHinge, target);

    }
}
