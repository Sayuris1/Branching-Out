using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ui : MonoBehaviour
{
    [SerializeField] bool _isMuted = false;
    [SerializeField] Image _mutedImage;
    [SerializeField] AudioMixer _mix;
    [SerializeField] Sprite _mutedSprite;
    [SerializeField] Sprite _openSprite;

   public void PressedQuit()
   {
        Application.Quit();
   }

    public void PressedStart()
    {                       
        GameManager.Instance.WinGame();
    }

    public void PressedMute()
    {
        if(_isMuted)
        {
            _mix.SetFloat("MainVolume", 0);
            _isMuted = false;
            _mutedImage.sprite = _openSprite;
        }
        else
        {
            _mix.SetFloat("MainVolume", -80);
            _isMuted = true;
            _mutedImage.sprite = _mutedSprite;
        }
    }
}