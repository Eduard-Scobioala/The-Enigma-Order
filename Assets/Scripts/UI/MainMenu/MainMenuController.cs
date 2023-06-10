using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenuController saveSlotsMenu;


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
        Debug.LogWarning("OptionButton not implemented");

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
