using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SaveSlotsMenuController : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenuController mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    private SaveSlot[] saveSlots;
    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    #region OnButtonPressed

    public void OnSaveSlotButtonPressed(SaveSlot saveSlot)
    {
        // disable all buttons
        DisableMenuButtons();

        // Update the selected profileId used for the data persistance
        DataPersistanceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if (!isLoadingGame)
        {
            // Create a new game only if coming from NewGame Button (initiate a clean GameData object)
            DataPersistanceManager.Instance.NewGame();
        }
        else
        {
            DataPersistanceManager.Instance.LoadSave();
        }

        // Save the Game before loading a scene
        //DataPersistanceManager.Instance.SaveGame();

        // Load Scenes (which will also load the game because of OnSceneLoaded() function)
        string scene = DataPersistanceManager.Instance.gameData.currentSceneName;
        Vector3 playerPosition = DataPersistanceManager.Instance.gameData.playerPosition;

        //SceneManager.LoadScene("Essential");
        var mode = isLoadingGame ? GameSceneManager.SwitchMode.LoadGame : GameSceneManager.SwitchMode.NewGame;
        GameSceneManager.Instance.InitSwitchScene(scene, playerPosition, mode);
    }

    public void OnBackButtonPressed()
    {
        mainMenu.ActivateMenu();
        DeactivateMenu();
    }

    #endregion

    #region Activate/Deactivate Menu

    public void ActivateMenu(bool isLoadingGame)
    {
        gameObject.SetActive(true);

        // Set Mode for entering the SaveSlotMenu
        this.isLoadingGame = isLoadingGame;

        // Load all the profiles
        Dictionary<string, GameData> profilesGameData = DataPersistanceManager.Instance.GetAllProfilesGameData();

        // Loop throught the profiles and set the slots
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            // Check if any GameData with the profileId exists
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);

            // Set the save slot, remains empty if no data was found for the profileId
            saveSlot.SetData(profileData);

            // Set the SaveSlots interactability
            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
            }
        }
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }

    #endregion
}
