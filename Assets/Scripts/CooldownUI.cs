using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Update()
    {
        float target = PlayerController.Instance.GetCooldownPercent();
        image.fillAmount = Mathf.Lerp(image.fillAmount, target,Time.deltaTime * 12);
    }
}
