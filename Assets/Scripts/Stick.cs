using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public bool sticky = false;
    [SerializeField] private bool startSticky;
    
    private Rigidbody2D _rb;
    [SerializeField] private CapsuleCollider2D capsuleCol;
    [SerializeField] private CircleCollider2D topCol;
    [SerializeField] private HingeJoint2D topHinge;
    
    [SerializeField] private CircleCollider2D bottomCol;
    [SerializeField] private HingeJoint2D bottomHinge;
    private List<Stick> _childs = new List<Stick>();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SetSticky(startSticky);
    }

    public void SetSticky(bool sticky)
    {
        this.sticky = sticky;

        topCol.enabled = sticky;
        bottomCol.enabled = sticky;
        capsuleCol.enabled = !sticky;
    }

    public void OnSomeoneStick(Stick parent)
    {
        SetSticky(true);
    }

    public void StickToTarget(HingeJoint2D hinge, Stick target)
    {
        Debug.Log(name + " sticked to " + target);
        hinge.enabled = true;
        hinge.connectedBody = target._rb;
        target.OnSomeoneStick(this);
        ParticleManager.Instance.Play(0, transform.TransformPoint(hinge.anchor));
        _childs.Add(target);
    }

    public bool IsChildOfThis(Stick stick)
    {
        foreach (var child in _childs)
        {
            if (stick == child || child.IsChildOfThis(stick))
                return true;
        }

        return false;
    }

    public void AddTorque(float torque)
    {
        _rb.AddTorque(torque);
        foreach (var child in _childs)
        {
            child.AddTorque(torque);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
        Gizmos.color = Color.yellow;
        
        foreach (var child in _childs)
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
        {
            Debug.Log("Can connect but its my relative");
            return;
        }
        if (!topHinge.connectedBody && col.otherCollider == topCol)
            StickToTarget(topHinge, target);
        else if (!bottomHinge.connectedBody && col.otherCollider == bottomCol)
            StickToTarget(bottomHinge, target);

    }
}
