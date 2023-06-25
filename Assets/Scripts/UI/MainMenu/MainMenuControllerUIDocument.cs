using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuControllerUIDocument : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset mainMenuTree;
    [SerializeField] private VisualTreeAsset optionMenuTree;
    [SerializeField] private UIDocument uiDocument;

    public Button newGameButton;
    public Button continueButton;
    public Button optionButton;
    public Button exitButton;

    void Start()
    {
        // Get reference to the root ui element
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        newGameButton = root.Q<Button>("newGameButton");
        continueButton = root.Q<Button>("continueButton");
        optionButton = root.Q<Button>("optionButton");
        exitButton = root.Q<Button>("exitButton");

        newGameButton.clicked += NewGameButtonPressed;
        continueButton.clicked += ContinueButtonPressed;
        optionButton.clicked += OptionButtonPressed;
        exitButton.clicked += ExitButtonPressed;

        // Bind functionality only there is data already saved
        if (!DataPersistanceManager.Instance.HasTheGameData())
        {
            continueButton.SetEnabled(false);
        }
    }

    void NewGameButtonPressed()
    {
        //// Create a new game ( create a new GameData object)
        //DataPersistanceManager.Instance.NewGame();

        //// Load Scenes (which will also load the game because of OnSceneLoaded() function)
        //SceneManager.LoadSceneAsync("SceneOne");
        //SceneManager.LoadSceneAsync("Essential", LoadSceneMode.Additive);

        //VisualElement optionMenu = optionMenuTree.CloneTree();
        //GetComponent<UIDocument>().rootVisualElement.Clear();
        //GetComponent<UIDocument>().rootVisualElement.Add(optionMenu);

        uiDocument.visualTreeAsset = optionMenuTree;
    }

    void ContinueButtonPressed()
    {
        // Load the Game
        SceneManager.LoadSceneAsync("SceneOne");
        SceneManager.LoadSceneAsync("Essential", LoadSceneMode.Additive);
    }

    void OptionButtonPressed()
    {
        Debug.LogWarning("OptionMenu not implemented yet!");
    }

    void ExitButtonPressed()
    {
        Application.Quit();
    }
}
