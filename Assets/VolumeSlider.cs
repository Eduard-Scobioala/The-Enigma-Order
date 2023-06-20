using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MUSIC,
        SFX
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
        UpdateSoundVolume();
    }

    private void Update()
    {
        UpdateSoundVolume();
    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MUSIC:
                AudioManager.Instance.musicVolume = volumeSlider.value;
                PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
                break;
            case VolumeType.SFX:
                AudioManager.Instance.SFXVolume = volumeSlider.value;
                PlayerPrefs.SetFloat("SFXVolume", volumeSlider.value);
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }

    private void UpdateSoundVolume()
    {
        switch (volumeType)
        {
            case VolumeType.MUSIC:
                AudioManager.Instance.musicVolume = PlayerPrefs.GetFloat("musicVolume", 1.0f);
                break;
            case VolumeType.SFX:
                AudioManager.Instance.SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }
}