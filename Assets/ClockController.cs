using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockController : Interactable
{
    [SerializeField] private GameObject letterInterface;
    [SerializeField] private TMP_Text letterText;

    private void Awake()
    {
        letterInterface.SetActive(false);
    }

    public override void Interact(Character character)
    {
        letterInterface.SetActive(true);
        letterText.text = GetCurrentTime();
        GameManager.instance.characterCanMove = false;
        GameManager.instance.canOpenInventory = false;
    }

    public void OnExitButtonPressed()
    {
        letterInterface.SetActive(false);
        GameManager.instance.characterCanMove = true;
        GameManager.instance.canOpenInventory = true;
    }

    private string GetCurrentTime()
    {
        DateTime currentTime = DateTime.Now;
        string formattedTime = currentTime.ToString("HH:mm");

        return formattedTime;
    }
}