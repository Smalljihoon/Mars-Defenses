using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [Header("오디오")]
    [SerializeField] AudioMixer audioMixer = null;
    [SerializeField] AudioSource effectSource = null;
    [SerializeField] AudioClip effectsound = null;

    [Header("슬라이더")]
    [SerializeField] Slider masterVol = null;
    [SerializeField] Slider effectVol = null;
    [SerializeField] Slider backgroundVol = null;

    [SerializeField] GameObject VolumeWindow = null;
    [SerializeField] GameObject ExplanationWindow = null;
    private bool isActivate = false;

    public void Start()
    {
        // playerprefs를 통해 저장된 볼륨값 가져오기
        if (PlayerPrefs.HasKey("masterVolvalue"))
        {
            masterVol.value = PlayerPrefs.GetFloat("masterVolvalue");
        }
        if (PlayerPrefs.HasKey("effectVolvalue"))
        {
            effectVol.value = PlayerPrefs.GetFloat("effectVolvalue");
        }
        if (PlayerPrefs.HasKey("BackgroundVolvalue"))
        {
            backgroundVol.value = PlayerPrefs.GetFloat("BackgroundVolvalue");
        }
    }

    // 볼륨 설정 버튼
    public void VolumeButton()
    {
        if (!isActivate)
        {
            VolumeWindow.SetActive(true);
            isActivate= true;
        }
        else
        {
            ExplanationWindow.SetActive(false);
            VolumeWindow.SetActive(true);
        }
    }
    // 게임설명 버튼
    public void ExplanationButton()
    {
        if (!isActivate)
        {
            ExplanationWindow.SetActive(true);
            isActivate = true;
        }
        else
        {
            VolumeWindow.SetActive(false);
            ExplanationWindow.SetActive(true);
        }
    }
    // 전체 볼륨 조절
    public void SetMasterVol(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("masterVolvalue", masterVol.value);    // playerprefs에 슬라이더 value값 저장
        PlayerPrefs.Save();
    }
    // 효과음 볼륨 조절
    public void SetEffectVol(float volume)
    {
        audioMixer.SetFloat("Effect", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("effectVolvalue", effectVol.value);
        PlayerPrefs.Save();
    }
    // 배경음 볼륨 조절
    public void SetBackground(float volume)
    {
        audioMixer.SetFloat("Background", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("BackgroundVolvalue", backgroundVol.value);
        PlayerPrefs.Save();
    }
    // 효과음 재생
    public void EffectsoundTest()
    {
        effectSource.clip = effectsound;
        effectSource.Play();
    }
    // 게임  시작 버튼
    public void GameStartOnclick()
    {
        SceneManager.LoadScene("Game");
    }
    // 게임 종료 버튼
    public void GameEndOnclick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
