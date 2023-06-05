using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button exitButton;

    void Start()
    {
        // Get reference to the root ui element
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        startButton = root.Q<Button>("newGameButton");
        optionButton = root.Q<Button>("optionButton");
        exitButton = root.Q<Button>("exitButton");

        startButton.clicked += StartButtonPressed;
        optionButton.clicked += OptionButtonPressed;
        exitButton.clicked += ExitButtonPressed;
    }

    void StartButtonPressed()
    {
        SceneManager.LoadScene("SceneOne");
        SceneManager.LoadScene("Essential", LoadSceneMode.Additive);
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
