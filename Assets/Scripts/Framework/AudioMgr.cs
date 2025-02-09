// // ********************************************************************************************
// //     /\_/\                           @file       AudioMgr.cs
// //    ( o.o )                          @brief     
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2025020213
// //   (___)___)                         @Copyright  Copyright (c) 2025, Basya
// // ********************************************************************************************

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AudioMgr
{
    private static AudioMgr _instance;

    public static AudioMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject soundManagerObject = new GameObject("SoundManagerObject");
                _instance = new AudioMgr(soundManagerObject);
                Object.DontDestroyOnLoad(soundManagerObject);
            }

            return _instance;
        }
    }

    public float MusicVolume => musicVolume;

    public float SfxVolume => sfxVolume;


    public float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
    public float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);


    private AudioSource musicSource;
    private List<AudioSource> sfxPool = new List<AudioSource>();

    private const int MaxSFXSources = 10;

    private GameObject audioGameObject;

    private AudioMgr(GameObject gameObject)
    {
        audioGameObject = gameObject;
        musicSource = audioGameObject.AddComponent<AudioSource>();
    }

    public void PlayMusic(string path)
    {
        AudioClip musicClip = Resources.Load<AudioClip>(path);
        if (musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.volume = MusicVolume;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("未找到音乐资源: " + path);
        }
    }

    //超级无敌石山，但是没时间了
    public void PlayFirstThenLoop(AudioClip firstClip, AudioClip loopClip)
    {
        if (firstClip == null || loopClip == null)
        {
            Debug.LogWarning("未找到音效资源");
            return;
        }

        musicSource.volume = MusicVolume;
        musicSource.clip = firstClip;
        musicSource.Play();

        UniTask.Create(async () =>
        {
            await UniTask.WaitUntil(() => !musicSource.isPlaying);
            musicSource.clip = loopClip;
            musicSource.loop = true;
            musicSource.Play();
        }).Forget();
    }

    public void PlaySFX(string path)
    {
        AudioClip sfxClip = Resources.Load<AudioClip>(path);
        if (sfxClip != null)
        {
            // 查找一个空闲的音频源
            AudioSource availableSource = GetAvailableSFXSource();

            if (availableSource != null)
            {
                availableSource.clip = sfxClip;
                availableSource.volume = SfxVolume;
                availableSource.Play();
            }
            else
            {
                Debug.LogWarning("音效池已满，无法播放更多音效: " + path);
            }
        }
        else
        {
            Debug.LogWarning("未找到音效资源: " + path);
        }
    }

    public void PlaySFX(string path, int range)
    {
        string realPath = path += $"{Random.Range(1, range + 1)}";
        PlaySFX(realPath);
    }

    private AudioSource GetAvailableSFXSource()
    {
        // 查找一个已经停止的音频源
        foreach (var source in sfxPool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // 如果没有空闲的音频源，创建一个新的
        if (sfxPool.Count < MaxSFXSources)
        {
            AudioSource newSFXSource = audioGameObject.AddComponent<AudioSource>();
            sfxPool.Add(newSFXSource);
            return newSFXSource;
        }

        // 如果音效池已满，返回 null
        return null;
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
        musicVolume = volume;
        musicSource.volume = MusicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("sfxVolume", volume);
        PlayerPrefs.Save();
        sfxVolume = volume;
        foreach (var source in sfxPool)
        {
            source.volume = SfxVolume;
        }
    }
}