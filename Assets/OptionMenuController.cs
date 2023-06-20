using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuController : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenuController mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;




    public void OnBackButtonPressed()
    {
        mainMenu.ActivateMenu();
        DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}
