using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class WinLoseUI : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup master;
    [SerializeField] private AudioSource win;
    [SerializeField] private AudioSource winBonus;
    [SerializeField] private AudioSource lose;

    [SerializeField] private Image winImg;
    [SerializeField] private Image winBonusImg;
    [SerializeField] private Image loseImg;


    [SerializeField] private CharacterController characterController;

    float timer = 0;
    private void Update()
    {
        if (characterController != null) return;
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        timer = 3;
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        characterController.OnEndGame += OnEndGame;
        characterController.OnLose += OnLose;
    }

    private void OnLose()
    {
        master.audioMixer.SetFloat("MasterVolume", -80);
        lose.Play();
        loseImg.gameObject.SetActive(true);
    }

    private void OnEndGame(bool hasGoldPot)
    {
        master.audioMixer.SetFloat("MasterVolume", -80);
        if (hasGoldPot)
        {
            winBonus.Play();
            winBonusImg.gameObject.SetActive(true);
        }
        else
        {
            win.Play();
            winImg.gameObject.SetActive(true);
        }
    }
}
