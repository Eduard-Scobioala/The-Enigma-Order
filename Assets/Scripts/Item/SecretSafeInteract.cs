using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecretSafeInteract : Interactable
{
    [SerializeField] private GameObject secretLetter;
    [SerializeField] private GameObject passwordInterface;
    [SerializeField] private TMP_InputField passwordField;

    string password = "";
    Animator animator;
    new Collider2D collider;
    public bool open = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        open = false;
        collider.enabled = true;
    }

    public override void Interact(Character character)
    {
        if (!open)
        {
            passwordInterface.SetActive(true);
            GameManager.instance.characterCanMove = false;
            GameManager.instance.canOpenInventory = false;
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

        if (passwordField.text.ToUpper() == password)
        {
            open = true;
            animator.SetTrigger("open");
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.unlockSafe, this.transform.position);

            collider.enabled = false;
            secretLetter.SetActive(true);
        }

        passwordField.text = "";
        passwordInterface.SetActive(false);
        GameManager.instance.characterCanMove = true;
        GameManager.instance.canOpenInventory = true;
    }

    private string GetCurrentPassword()
    {
        return "EUREKA";
    }
}
