using System;
using TMPro;
using UnityEngine;

public class CabinetInteract : Interactable
{
    [SerializeField] private GameObject secretLetter;
    [SerializeField] private GameObject passwordInterface;
    [SerializeField] private TMP_InputField passwordField;

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
            secretLetter.SetActive(false);
            animator.SetTrigger("close");
            open = false;
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
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.openCabinet, this.transform.position);
            secretLetter.SetActive(true);
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
