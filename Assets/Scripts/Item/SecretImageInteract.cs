using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecretImageInteract : Interactable
{
    [SerializeField] private GameObject secretSafe;
    [SerializeField] private GameObject secretLetter;

    Animator animator;
    bool open = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact(Character character)
    {
        if (!open)
        {
            open = true;
            animator.SetTrigger("open");
            secretSafe.SetActive(true);
        }
        else
        {
            open = false;
            animator.SetTrigger("close");
            StartCoroutine(CloseSafe());
        }
    }

    IEnumerator CloseSafe()
    {
        yield return new WaitForSeconds(0.7f);
        secretSafe.SetActive(false);
        secretLetter.SetActive(false);
    }
}
