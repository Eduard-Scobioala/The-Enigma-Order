using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;

public class FMODEvents : MonoBehaviour
{
    [field: Header("MainMenuMusic")]
    [field: SerializeField] public EventReference mainMenuMusic { get; private set; }

    [field: Header("GameMusic")]
    [field: SerializeField] public EventReference gameMusic { get; private set; }
    public EventReference backgroundMusic { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }

    [field: Header("Cabinet SFX")]
    [field: SerializeField] public EventReference openCabinet { get; private set; }

    [field: Header("Safe SFX")]
    [field: SerializeField] public EventReference unlockSafe { get; private set; }

    [field: Header("Typing SFX")]
    [field: SerializeField] public EventReference typingEffect { get; private set; }

    public static FMODEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        Instance = this;

        string name = SceneManager.GetActiveScene().name;
        if (name == "MainMenu" )
        {
            backgroundMusic = mainMenuMusic;
        }
        else if (name == "Prologue") {
            return;
        }
        else
        {
            backgroundMusic = gameMusic;
        }
    }
}