using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    //singleton instance
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<AudioManager>();
                    singletonObject.name = typeof(AudioManager).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    [Header("Outputs")]
    public AudioSource masterOutput;//muzik
    public AudioSource sfxOutput;//ses efektleri
    public float maxOutputVolume = 1f;//muzigin max seviyesi
    public float startDuration = 2f;//acilis yumusakligi ayari
    [Header("Sliders")]
    public Slider masterSlider;
    public Slider sfxSlider;
    [Header("Musics")]
    public AudioClip music0;
    public AudioClip music1;
    [Header("Sound Effects")]
    public AudioClip sound0;
    public AudioClip sound1;
    public AudioClip sound2;
    private void Start()
    {
        StartVolumeEffect();//muzigi yumusak sekilde ac

        //baslangicta slider degerlerini ayarlar
        StartCoroutine(StartSlider(masterSlider,masterOutput));
        StartCoroutine(StartSlider(sfxSlider,sfxOutput));
    }
    public void StartVolumeEffect()
    {
        //sahne acildiginda muzigin sesini smooth sekilde acar
        masterOutput.volume = 0;
        ChangeVolumeMaster(maxOutputVolume, startDuration);
    }
    public void ChangeMusic(AudioClip clip)
    {
        //calan muzigi degistirir
        StartCoroutine(ChangeMusicEnum(clip));
    }
    public void ChangeVolumeMaster(float targetValue, float duration)
    {
        //muzigin sesini artirip azalt
        masterOutput.DOFade(targetValue, duration);
    }
    public void ChangeVolumeMasterSlider(Slider slider)
    {
        //muzigin sesini artirip azalt
        masterOutput.DOFade(slider.value, 0.5f);
    }
    public void ChangeVolumeSFX(float targetValue, float duration)
    {
        //ses efektlerinin sesini artirip azalt
        sfxOutput.DOFade(targetValue, duration);
    }
    public void ChangeVolumeSFXSlider(Slider slider)
    {
        //ses efektlerinin sesini artirip azalt
        sfxOutput.DOFade(slider.value, 0.5f);
    }
    public void PlaySoundEffect(AudioClip audioClip)
    {
        //ses efekti cal
        sfxOutput.PlayOneShot(audioClip, sfxSlider.value);
    }
    IEnumerator ChangeMusicEnum(AudioClip clip)
    {
        ChangeVolumeMaster(0f, 1f);//sesi kis
        yield return new WaitForSeconds(1f);//sesin kisilmasini bekle

        masterOutput.Stop();//klibi durdur
        masterOutput.clip = clip;//yeni klip koy
        masterOutput.Play();//klibi cal

        masterOutput.loop = true;//loop yap
        ChangeVolumeMaster(1f, 1f);//sesi ac
    }
    IEnumerator StartSlider(Slider slider, AudioSource output)
    {
        float duration = 2f; //guncelleme suresi
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            //slider seviyesini guncelle
            slider.value = output.volume;

            yield return null;
        }

    }
}
