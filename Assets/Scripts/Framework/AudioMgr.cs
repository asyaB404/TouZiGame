// // ********************************************************************************************
// //     /\_/\                           @file       AudioMgr.cs
// //    ( o.o )                          @brief     
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2025020213
// //   (___)___)                         @Copyright  Copyright (c) 2025, Basya
// // ********************************************************************************************

using System.Collections.Generic;
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


    public float musicVolume = 1f;
    public float sfxVolume = 1f;


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

    public void PlayFirstThenLoop(string path1, string path2)
    {
        AudioClip firstClip = Resources.Load<AudioClip>(path1);
        AudioClip loopClip = Resources.Load<AudioClip>(path2);
        if (firstClip == null || loopClip == null)
        {
            Debug.LogWarning("未找到音效资源: " + path1 + ", " + path2);
            return;
        }

        // 播放第一个音频
        musicSource.clip = firstClip;
        musicSource.PlayScheduled(AudioSettings.dspTime);

        // 计算第一个音频播放结束的时间点
        double startTime = AudioSettings.dspTime + firstClip.length;

        // 预定第二个音频的播放
        musicSource.clip = loopClip;
        musicSource.loop = true; 
        musicSource.PlayScheduled(startTime);
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
        musicVolume = volume;
        musicSource.volume = MusicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        foreach (var source in sfxPool)
        {
            source.volume = SfxVolume;
        }
    }
}