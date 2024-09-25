using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [Header("�����")]
    [SerializeField] AudioMixer audioMixer = null;
    [SerializeField] AudioSource effectSource = null;
    [SerializeField] AudioClip effectsound = null;

    [Header("�����̴�")]
    [SerializeField] Slider masterVol = null;
    [SerializeField] Slider effectVol = null;
    [SerializeField] Slider backgroundVol = null;

    [SerializeField] GameObject VolumeWindow = null;
    [SerializeField] GameObject ExplanationWindow = null;
    private bool isActivate = false;

    public void Start()
    {
        // playerprefs�� ���� ����� ������ ��������
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

    // ���� ���� ��ư
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
    // ���Ӽ��� ��ư
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
    // ��ü ���� ����
    public void SetMasterVol(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("masterVolvalue", masterVol.value);    // playerprefs�� �����̴� value�� ����
        PlayerPrefs.Save();
    }
    // ȿ���� ���� ����
    public void SetEffectVol(float volume)
    {
        audioMixer.SetFloat("Effect", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("effectVolvalue", effectVol.value);
        PlayerPrefs.Save();
    }
    // ����� ���� ����
    public void SetBackground(float volume)
    {
        audioMixer.SetFloat("Background", Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("BackgroundVolvalue", backgroundVol.value);
        PlayerPrefs.Save();
    }
    // ȿ���� ���
    public void EffectsoundTest()
    {
        effectSource.clip = effectsound;
        effectSource.Play();
    }
    // ����  ���� ��ư
    public void GameStartOnclick()
    {
        SceneManager.LoadScene("Game");
    }
    // ���� ���� ��ư
    public void GameEndOnclick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
