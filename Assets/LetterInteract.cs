using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterInteract : Interactable
{
    [SerializeField] private GameObject letterInterface;

    private void Awake()
    {
        letterInterface.SetActive(false);
    }

    public override void Interact(Character character)
    {
        letterInterface.SetActive(true);
        GameManager.instance.characterCanMove = false;
        GameManager.instance.canOpenInventory = false;
    }

    public void OnExitButtonPressed()
    {
        letterInterface.SetActive(false);
        GameManager.instance.characterCanMove = true;
        GameManager.instance.canOpenInventory = true;
    }
}
