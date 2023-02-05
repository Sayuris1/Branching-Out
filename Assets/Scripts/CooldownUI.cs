using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private RectTransform trans;

    private void Update()
    {
        float x = Mathf.Lerp(trans.localScale.x, PlayerController.Instance.GetCooldownPercent(),Time.deltaTime * 5);
        trans.localScale = new Vector3(x, 1, 1);
    }
}
