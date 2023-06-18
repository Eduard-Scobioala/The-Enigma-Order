using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CabinetInteract : Interactable
{
    [SerializeField] private GameObject passwordInterface;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] AudioClip onOpenAudio;

    Animator animator;
    string password = "";

    bool open = false;

    private void Awake()
    {
        passwordInterface.SetActive(false);
        animator = GetComponent<Animator>();
    }

    public override void Interact(Character character)
    {
        

        if (!open)
        {
            passwordInterface.SetActive(true);
            GameManager.instance.characterCanMove = false;
            GameManager.instance.canOpenInventory = false;
        }
        else
        {
            open = false;
            animator.SetTrigger("close");
        }
    }

    public void OnExitButtonPressed()
    {
        // Reset values for future interact
        passwordInterface.SetActive(false);
        GameManager.instance.characterCanMove = true;
        GameManager.instance.canOpenInventory = true;
    }

    public void OnPasswordInputVerify()
    {
        password = GetCurrentPassword();

        if (passwordField.text == password)
        {
            open = true;
            animator.SetTrigger("open");
            AudioManager.instance.Play(onOpenAudio);
        }

        passwordField.text = "";
        passwordInterface.SetActive(false);
        GameManager.instance.characterCanMove = true;
        GameManager.instance.canOpenInventory = true;
    }

    private string GetCurrentPassword()
    {
        DateTime currentTime = DateTime.Now;
        string formattedTime = currentTime.ToString("HH:mm");

        return formattedTime;
    }
}
