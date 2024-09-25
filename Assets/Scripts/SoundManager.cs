using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer = null;
    [SerializeField] Slider MasterValue;
    [SerializeField] Slider EffectValue;
    [SerializeField] Slider BackgroundValue;

    public void Start()
    {
        if(PlayerPrefs.HasKey("masterVolvalue"))
        {
            MasterValue.value = PlayerPrefs.GetFloat("masterVolvalue");
        }
        if(PlayerPrefs.HasKey("effectVolvalue"))
        {
            EffectValue.value = PlayerPrefs.GetFloat("effectVolvalue");
        }
        if(PlayerPrefs.HasKey("BackgroundVolvalue"))
        {
            BackgroundValue.value = PlayerPrefs.GetFloat("BackgroundVolvalue");
        }
    }

    public void SetMasterVol(float volume)
    {
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.DeleteKey("aa");

        mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetEffectVol(float volume)
    {
        mixer.SetFloat("Effect", Mathf.Log10(volume) * 20);
    }

    public void SetBackground(float volume)
    {
        mixer.SetFloat("Background", Mathf.Log10(volume) * 20);
    }
}
