using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterInteract : Interactable
{
    [SerializeField] private GameObject LetterInterface;

    private void Awake()
    {
        LetterInterface.SetActive(false);
    }

    public override void Interact(Character character)
    {
        LetterInterface.SetActive(true);
        GameManager.instance.characterCanMove = false;
    }

    public void OnExitButtonPressed()
    {
        LetterInterface.SetActive(false);
        GameManager.instance.characterCanMove = true;
    }
}
