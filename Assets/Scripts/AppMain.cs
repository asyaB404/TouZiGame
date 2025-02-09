// // ********************************************************************************************
// //     /\_/\                           @file       AppMain.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2025020917
// //   (___)___)                         @Copyright  Copyright (c) 2025, Basya
// // ********************************************************************************************


using System;
using GamePlay.Core;
using UnityEngine;

public class AppMain : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioClip[] musicClips;
 
    private void Awake()
    {
        gameManager.Init();
    }

    private void Start()
    {
        AudioMgr.Instance.PlayFirstThenLoop(musicClips[0], musicClips[1]);
    }
}