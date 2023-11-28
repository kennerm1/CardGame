using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeCont : MonoBehaviour
{
    //this script creates volume control and file IO
    //after the first time playing, the audio input gets saved to a file on the computer and then accessed by the game each time the user plays on that computer

    [SerializeField] string volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] float multiplier = 30f;
    [SerializeField] Toggle toggle;

    private bool disableToggleEvent;

    //accesses file that holds slider and toggle values
    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
        toggle.onValueChanged.AddListener(HandleToggleValueChanged);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }

    //stores volume slider value
    private void HandleSliderValueChanged(float value)
    {
        mixer.SetFloat(volumeParameter, Mathf.Log10(value) * multiplier);
        disableToggleEvent = true;
        toggle.isOn = slider.value > slider.minValue;
        disableToggleEvent = false;
    }

    //stores mute/unmute toggle value
    private void HandleToggleValueChanged(bool enableSound)
    {
        if (disableToggleEvent)
        {
            return;
        }

        if (enableSound)
        {
            slider.value = slider.maxValue;
        }
        else
        {
            slider.value = slider.minValue;
        }
    }

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
    }
}
