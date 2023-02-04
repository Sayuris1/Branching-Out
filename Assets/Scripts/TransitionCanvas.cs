using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TransitionCanvas : MonoBehaviour
{
    [SerializeField] float transitionSpeed;
    [SerializeField] private RectTransform left;
    [SerializeField] private RectTransform right;

    public static TransitionCanvas Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);
        left.DOScaleX(0, transitionSpeed);
        right.DOScaleX(0, transitionSpeed);
    }

    public void Transition(TweenCallback onComplete)
    {
        left.DOScaleX(1, transitionSpeed).OnComplete(onComplete);
        right.DOScaleX(1, transitionSpeed).OnComplete(onComplete);
    }
    
    
}
