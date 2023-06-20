using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenuController saveSlotsMenu;
    [SerializeField] private OptionMenuController optionMenu;


    #region OnButtonPressed

    public void OnNewGameButtonPressed()
    {
        this.DeactivateMenu();
        saveSlotsMenu.ActivateMenu(false);
    }

    public void OnLoadGameButtonPressed()
    {
        this.DeactivateMenu();
        saveSlotsMenu.ActivateMenu(true);
    }

    public void OnOptionButtonPressed()
    {
        this.DeactivateMenu();
        optionMenu.ActivateMenu(true);
    }

    public void OnExitButtonPressed()
    {
        DataPersistanceManager.Instance.onExitButtonPressed = true;
        Application.Quit();
    }

    #endregion

    #region Activate/Deactivate Menu
    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    #endregion
}
